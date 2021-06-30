using IdentityServer4;
using IdentityServer4.Models;
using Klika.AuthApi.Model.Constants.IdentityConfig;
using System;
using System.Collections.Generic;

namespace Klika.AuthApi.Configuration
{
    public class IdentityConfiguration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetResourceApis()
        {
            return new List<ApiResource>
            {
                new ApiResource(name: InternalApis.KlikaApi, displayName: "Klika Resource API") { Scopes = new List<string>() { InternalApis.KlikaApi } }
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new[]
            {
                new ApiScope(name: InternalApis.KlikaApi,   displayName: "Klika Resource Api Access")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client()
                {
                    ClientId = InternalClients.Mobile,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret(Environment.GetEnvironmentVariable("mobile_client_secret").Sha256()) },
                    AllowedScopes = new List<string>
                    {
                        InternalApis.KlikaApi,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess
                    },
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                },
                new Client()
                {
                    ClientId = InternalClients.Web,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret(Environment.GetEnvironmentVariable("web_client_secret").Sha256()) },
                    AllowedScopes = new List<string>
                    {
                        InternalApis.KlikaApi,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess
                    },
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                }
            };
        }
    }
}
