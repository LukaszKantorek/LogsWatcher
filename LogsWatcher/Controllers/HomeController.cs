using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

namespace LogsReader.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //await RefreshTokenAsync();

            //var accessToken = await HttpContext.GetTokenAsync("access_token");
            //var idToken = await HttpContext.GetTokenAsync("id_token");
            //var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            return View();
        }

        public async Task LogoutAsync()
        {
            Response.Cookies.Delete("idsrv");
            Response.Cookies.Delete("idsrv.session");

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }

        [HttpGet]
        public string GetUserName()
        {
            var users = User
                .Claims
                .ToList();

            var userName = users
                .Where(c => c.Type == "email")
                .First()
                .Value;

            return userName;
        }

        private async Task RefreshTokenAsync()
        {
            var authorizationServerInformation =
                await DiscoveryClient.GetAsync("http://localhost:59418");

            var client = new TokenClient(authorizationServerInformation.TokenEndpoint, "logsreader_code", "secret");

            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            var tokenResponse = await client.RequestRefreshTokenAsync(refreshToken);
            var idToken = await HttpContext.GetTokenAsync("id_token");

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

            var authenticationInformation = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            authenticationInformation.Properties.StoreTokens(tokens);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                authenticationInformation.Principal,
                authenticationInformation.Properties);
        }
    }
}
