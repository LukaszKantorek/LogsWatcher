using System;
using System.Globalization;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace LogsWatcher.Tokens
{
    public interface ITokenRefresher
    {
        Task RefreshTokenAsync(HttpContext context);
    }

    public class TokenRefresher : ITokenRefresher
    {
        private readonly IConfiguration _configuration;

        public TokenRefresher(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task RefreshTokenAsync(HttpContext context)
        {
            var authorityService = _configuration
                .GetSection("Services")
                .GetSection("AuthorityService")
                .Value;

            var authorizationServerInformation =
                await DiscoveryClient.GetAsync(authorityService);

            var client = new TokenClient(authorizationServerInformation.TokenEndpoint, "logsreader_code", "secret");

            var refreshToken = await context.GetTokenAsync("refresh_token");

            var tokenResponse = await client.RequestRefreshTokenAsync(refreshToken);
            var idToken = await context.GetTokenAsync("id_token");

            var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResponse.ExpiresIn);

            var tokens = new[]
            {
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.IdToken,
                    Value = idToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.AccessToken,
                    Value = tokenResponse.AccessToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.RefreshToken,
                    Value = tokenResponse.RefreshToken
                },
                new AuthenticationToken
                {
                    Name = "expires_at",
                    Value = expiresAt.ToString("o", CultureInfo.InvariantCulture)
                }
            };

            var authenticationInformation = await context.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            authenticationInformation.Properties.StoreTokens(tokens);

            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                authenticationInformation.Principal,
                authenticationInformation.Properties);
        }
    }
}
