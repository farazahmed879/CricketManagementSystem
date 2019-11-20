using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CricketApp.Data;
using CricketApp.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using RazorHtmlToPdfDemo.Services;
using Swashbuckle.AspNetCore.Swagger;
using WebApp.IServices;
using WebApp.Services;
using Ground = WebApp.Services.Ground;

namespace WebApp
{
    public class Startup
    {
        private readonly IHostingEnvironment hostingEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            this.hostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IRazorViewToStringRenderer, RazorViewToStringRenderer>();
            services.AddIdentity<ApplicationUser, ApplicationUserRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
            })
               .AddEntityFrameworkStores<CricketContext>()
               .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = false;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                // If the LoginPath isn't set, ASP.NET Core defaults 
                // the path to /Account/Login.
                options.LoginPath = "/Account/Login";
                // If the AccessDeniedPath isn't set, ASP.NET Core defaults 
                // the path to /Account/AccessDenied.
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.Configure<MvcOptions>(options =>
            {
            //#if RELEASE
            //            options.Filters.Add(new RequireHttpsAttribute());
            //#endif
            });

            services.AddAutoMapper();

            services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");

            services.AddMvc();
          
            services.AddDbContextPool<CricketContext>(options =>
            {
                //string connectionString = hostingEnvironment.IsDevelopment() ?
                //    Configuration.GetConnectionString("Server=LOCALHOST; Database=ScoreExecDb; User=root; Password=Super@samad123;") :
                //    Configuration.GetConnectionString("Server=LOCALHOST; Database=ScoreExecDb; User=root; Password=Super@samad123;");
                var connectionString = Configuration["ConnectionStrings:CricketAppConnection"];
                options
                .UseMySql(connectionString,
                mySqlOptions =>
                {
                    mySqlOptions.ServerVersion(new Version(5, 7, 26), Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql);

                })
                .EnableSensitiveDataLogging()
                .ConfigureWarnings(i => i.Log());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Score Executive", Version = "v1" });
            });

            services.AddScoped<IPlayers, Players>();
            services.AddScoped<ITournaments, Tournaments>();
            services.AddScoped<IMatches, Matches>();
            services.AddScoped<ISeries, Series>();
            services.AddScoped<ITeams, Teams>();
            services.AddScoped<IMatchSummary, MatchSummary>();
            services.AddScoped<IGround, Ground>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //var rootPath = env.WebRootPath;
            //var acmeChallengePath =
            //    Path.Combine(rootPath, @".well-known\acme-challenge");
            //app.UseDirectoryBrowser(new DirectoryBrowserOptions
            //{
            //    FileProvider = new PhysicalFileProvider(acmeChallengePath),
            //    RequestPath = new PathString("/.well-known/acme-challenge"),
            //});

            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    ServeUnknownFileTypes = true
            //});


            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseHsts();
                
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Score Executive");
            });
        }
    }
}
