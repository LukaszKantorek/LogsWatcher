using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace LogsReader.AuthenticationServer
{
    public class InMemoryConfigurations
    {
        public static IEnumerable<ApiResource> GetApiRecources()
        {
            return new[]
            {
                new ApiResource("logsreader", "Logs Reader"),
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client()
                {
                    ClientId = "logsreader",
                    ClientSecrets = new [] {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = new []{ "logsreader" }
                },
                new Client()
                {
                    ClientId = "logsreader_implicit",
                    ClientSecrets = new [] {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = new []{
                        "logsreader",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    RedirectUris = new [] {"http://localhost:61148/signin-oidc" },
                    PostLogoutRedirectUris = new [] {"http://localhost:61148/signout-callback-oidc" },
                    AllowAccessTokensViaBrowser = true
                },
                new Client()
                {
                    ClientId = "logsreader_code",
                    ClientSecrets = new [] {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AllowedScopes = new []{
                        "logsreader",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email
                    },
                    AllowOfflineAccess = true,
                    RedirectUris = new [] {"http://localhost:61148/signin-oidc" },
                    PostLogoutRedirectUris = new [] {"http://localhost:61148/signout-callback-oidc" },
                    AllowAccessTokensViaBrowser = true

                }
            };
        }

        public static IEnumerable<TestUser> GetUsers()
        {
            return new[]
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "john.kowalski@gmail.com",
                    Password = "password1",
                    Claims = new[]
                    {
                        new Claim("email", "john.kowalski@gmail.com")
                    }
                }
            };
        }
    }
}
