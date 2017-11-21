using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using InteractiveCV.Data;
using InteractiveCV.Models;
using InteractiveCV.Services;
using Microsoft.Extensions.Logging;

namespace InteractiveCV
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {

                //password settings
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 4;

                //lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                //User Settings
                options.User.RequireUniqueEmail = true;

                //login settings
                options.SignIn.RequireConfirmedEmail = false;

            });

            services.ConfigureApplicationCookie(options =>
           {
               //Cookie settings
               options.CookieHttpOnly = true;
               options.Cookie.Expiration = TimeSpan.FromDays(30);
               options.LoginPath = "/Account/Login";
               options.LogoutPath = "/Account/Logout";
               options.AccessDeniedPath = "/Account/AccessDenied";
               options.SlidingExpiration = true;

           });

            services.AddTransient<IEmailSender, EmailSender>();

            //loading Mailkit configuration from appsettings and allowing injection
            services.AddSingleton<IEmailConfiguration>(Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
            services.AddTransient<IEmailService, EmailService>();
            services.AddMvc();

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ApplicationDbContext context)
        {
            app.UseDeveloperExceptionPage();
            app.UseBrowserLink();
            app.UseDatabaseErrorPage();

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
                //app.UseDatabaseErrorPage();
                int i = 0;
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            loggerFactory.AddConsole(Configuration.GetSection("Logging")).AddDebug();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //DBInitializer.Initialize(context);
        }
    }
}
