using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SeaBattle.Application.Services;
using SeaBattle.Application.Services.Interfaces;
using SeaBattle.Domain.Entities;
using SeaBattle.Domain.Interfaces;
using SeaBattle.Infrastructure;
using SeaBattle.Infrastructure.CustomIdentityProvider;
using SeaBattle.Infrastructure.Repositories;
using SeaBattle.Infrastructure.Serialization;
using SeaBattle.UI.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace SeaBattle.MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "SeaBattle";
                    options.LoginPath = new PathString("/Auth/Login");
                    options.LogoutPath = new PathString("/Auth/Logout");
                    options.ExpireTimeSpan = new TimeSpan(7, 0, 0, 0);
                });

            services.AddIdentity<User, Role>()
                .AddDefaultTokenProviders();

            services.AddControllersWithViews();  

            services.AddSingleton<IConfiguration>(provider => Configuration);

            services.Configure<AppOptions>(Configuration.GetSection(nameof(AppOptions)));
            services.AddSingleton<IAppOptions>(options => options.GetService<IOptions<AppOptions>>().Value);

            services.AddScoped<IUserStore<User>, CustomUserStore>();
            services.AddScoped<IRoleStore<Role>, CustomRoleStore>();

            if (Configuration.GetValue<bool>("AppOptions:IsSerializable"))
            {
                var jsonDataPath = Configuration.GetValue<string>("AppOptions:JsonDataPath");
                services.AddSerializationContext<SeaBattleSerializationContext>(jsonDataPath);
                services.AddScoped(typeof(IRepository<>), typeof(SerializationRepository<>));
            }
            else
            {
                services.AddScoped(typeof(IRepository<>), typeof(ORMRepository<>));
                services.AddScoped<SeaBattleORMContext>();
            }

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBoardService, BoardService>();
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<IShipService, ShipService>();
            services.AddScoped<ISupportAbilityService, SupportAbilityService>();
            services.AddScoped<IBattleAbilityService, BattleAbilityService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
