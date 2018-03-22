using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using LogsWatcher.Database;
using LogsWatcher.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LogsWatcher
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            string connectionString = Configuration.GetConnectionString("LogsDatabaaseConnectionString");
            services.AddDbContext<LogsDatabaseContext>(x => x.UseSqlServer(connectionString));
            services.AddTransient<ILogsRepository, LogsRepository>();
            services.AddTransient<ITokenRefresher, TokenRefresher>();

            services.AddMvc();

            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();

            services.AddAuthentication(options => {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie()
                .AddOpenIdConnect(options => {
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.SignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.RequireHttpsMetadata = false;
                    options.Authority = Configuration
                        .GetSection("Services")
                        .GetSection("AuthorityService")
                        .Value;
                    options.ClientId = "logsreader_code";
                    options.ClientSecret = "secret";
                    options.SaveTokens = true;
                    options.ResponseType = "id_token code";
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.Scope.Add("logsreader");
                    options.Scope.Add("offline_access");
                    options.Scope.Add("email");
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
