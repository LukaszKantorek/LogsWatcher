using System.Linq;
using System.Threading.Tasks;
using LogsWatcher.Tokens;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LogsWatcher.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenRefresher _refresher;

        public HomeController(IConfiguration configuration, ITokenRefresher refresher)
        {
            _configuration = configuration;
            _refresher = refresher;
        }
        public IActionResult Index()
        {
            //var accessTokenOld = HttpContext.GetTokenAsync("access_token");
            //var idTokenOld = HttpContext.GetTokenAsync("id_token");
            //var refreshTokenOld = HttpContext.GetTokenAsync("refresh_token");

            //_refresher.RefreshTokenAsync(HttpContext);

            //var accessToken = HttpContext.GetTokenAsync("access_token");
            //var idToken = HttpContext.GetTokenAsync("id_token");
            //var refreshToken = HttpContext.GetTokenAsync("refresh_token");

            return View();
        }

        public async Task LogoutAsync()
        {
            Response.Cookies.Delete("idsrv");
            Response.Cookies.Delete("idsrv.session");

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public string GetUserName()
        {
            var users = User
                .Claims
                .ToList();

            var userName = users
                .First(c => c.Type == "email")
                .Value;

            return userName;
        }
    }
}
