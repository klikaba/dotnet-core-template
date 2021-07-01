using AutoMapper;
using IdentityModel.Client;
using Klika.AuthApi.Model.DTOs;
using Klika.AuthApi.Model.Entities;
using Klika.AuthApi.Service.User.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Klika.AuthApi.Service.User
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(
                IMapper mapper,
                UserManager<ApplicationUser> userManager,
                IConfiguration config,
                IHttpClientFactory clientFactory,
                ILogger<UserService> logger)
        {
            _userManager = userManager;
            _config = config;
            _clientFactory = clientFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IdentityResult> Register(ApplicationUserDTO user)
        {
            try
            {
                ApplicationUser applicationUser = _mapper.Map<ApplicationUserDTO, ApplicationUser>(user);
                var identityResult = await _userManager.CreateAsync(applicationUser, user.Password).ConfigureAwait(false);

                if (identityResult.Succeeded)
                {
                    await _userManager.AddClaimsAsync(applicationUser, new List<Claim>() {
                        new Claim("email", applicationUser.Email)
                    }).ConfigureAwait(false);
                    await _userManager.AddToRolesAsync(applicationUser, new List<string>() { "admin" })
                                      .ConfigureAwait(false); // Define user roles on registration
                }

                return identityResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(Register));
                throw;
            }
        }

        public async Task<TokenResponse> Token(TokenRequestDTO tokenRequest)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var cache = new DiscoveryCache(_config["AuthApiUrl"]);
                var disco = await cache.GetAsync()
                                       .ConfigureAwait(false);
                if (disco.IsError)
                    throw new Exception(disco.Error);
                switch (tokenRequest.GrantType)
                {
                    case "password":
                        var passwordFlow = await client.RequestPasswordTokenAsync(new PasswordTokenRequest()
                        {
                            Address = disco.TokenEndpoint,
                            ClientId = tokenRequest.ClientId,
                            ClientSecret = tokenRequest.ClientSecret,
                            Scope = tokenRequest.Scope,
                            UserName = tokenRequest.Username,
                            Password = tokenRequest.Password
                        }).ConfigureAwait(false);
                        return passwordFlow;
                    case "client_credentials":
                        var clientCredentialsFlow = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
                        {
                            Address = disco.TokenEndpoint,
                            ClientId = tokenRequest.ClientId,
                            ClientSecret = tokenRequest.ClientSecret,
                            Scope = tokenRequest.Scope,
                        }).ConfigureAwait(false);
                        return clientCredentialsFlow;
                    default:
                        throw new Exception("grant_type is not supported");
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(Token));
                throw;
            }
        }

        public async Task<TokenResponse> RefreshToken(TokenRequestDTO tokenRequest)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var cache = new DiscoveryCache(_config["AuthApiUrl"]);
                var disco = await cache.GetAsync()
                                       .ConfigureAwait(false);
                if (disco.IsError)
                    throw new Exception(disco.Error);

                var refreshToken = await client.RequestRefreshTokenAsync(new RefreshTokenRequest()
                {
                    Address = disco.TokenEndpoint,
                    ClientId = tokenRequest.ClientId,
                    ClientSecret = tokenRequest.ClientSecret,
                    RefreshToken = tokenRequest.RefreshToken
                }).ConfigureAwait(false);
                return refreshToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(RefreshToken));
                throw;
            }
        }

        public async Task<TokenRevocationResponse> RevokeToken(TokenRequestDTO tokenRequest)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var cache = new DiscoveryCache(_config["AuthApiUrl"]);
                var disco = await cache.GetAsync()
                                       .ConfigureAwait(false);
                if (disco.IsError)
                    throw new Exception(disco.Error);

                var revokeResult = await client.RevokeTokenAsync(new TokenRevocationRequest
                {
                    Address = disco.RevocationEndpoint,
                    ClientId = tokenRequest.ClientId,
                    ClientSecret = tokenRequest.ClientSecret,
                    Token = tokenRequest.RefreshToken
                }).ConfigureAwait(false);
                return revokeResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(RevokeToken));
                throw;
            }
        }
    }
}
