using IdentityModel.Client;
using Klika.AuthApi.Model.DTOs;
using Klika.AuthApi.Service.User.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Klika.AuthApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<IdentityController> _logger;

        public IdentityController(IUserService userService, 
                                  ILogger<IdentityController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<IdentityResult>> Register([FromBody] ApplicationUserDTO user)
        {
            try
            {
                IdentityResult result = await _userService.Register(user)
                                                          .ConfigureAwait(false);

                if (result.Succeeded)
                    return Ok(result);
                return Conflict(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST:/register");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public async Task<IActionResult> Token([FromBody] TokenRequestDTO tokenRequest)
        {
            try
            {
                TokenResponse result = await _userService.Token(tokenRequest)
                                                        .ConfigureAwait(false);
                if (!result.IsError)
                    return Ok(result.Json);
                return Conflict(result.Json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST:/token");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [AllowAnonymous]
        [HttpPost("token/refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequestDTO tokenRequest)
        {
            try
            {
                TokenResponse result = await _userService.RefreshToken(tokenRequest)
                                                         .ConfigureAwait(false);
                if (!result.IsError)
                    return Ok(result.Json);
                return Conflict(result.Json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST:/refresh");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [AllowAnonymous]
        [HttpPost("token/revoke")]
        public async Task<ActionResult<TokenRevocationResponse>> RevokeToken([FromBody] TokenRequestDTO tokenRequest)
        {
            try
            {
                TokenRevocationResponse result = await _userService.RevokeToken(tokenRequest)
                                                                   .ConfigureAwait(false);
                if (!result.IsError)
                    return Ok();
                return Conflict(result.Json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST:/revoke");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
