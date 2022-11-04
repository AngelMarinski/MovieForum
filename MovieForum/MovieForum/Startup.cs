using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MovieForum.Data;
using MovieForum.Services;
using MovieForum.Services.Interfaces;
using MovieForum.Services.Services;
using MovieForum.Web.Helpers;
using MovieForum.Web.MappingConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Webgentle.Services.Models;

namespace MovieForum
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
                options.LoginPath = "/Auth/Login";
                options.ExpireTimeSpan = TimeSpan.FromDays(14);
                options.SlidingExpiration = true;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role,"Admin"));
                options.AddPolicy("User", policy => policy.RequireClaim(ClaimTypes.Role,"User"));
            });

            services.AddDbContext<MovieForumContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
                options.EnableSensitiveDataLogging();
            });

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });



            services.AddControllersWithViews();
            services.AddSwaggerGen();
            services.AddControllers();
            services.AddAutoMapper(cfg => cfg.AddProfile<MovieForumProfile>());

            services.AddScoped<ITagServices, TagServices>();
            services.AddScoped<IMoviesServices, MoviesServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<ICommentServices, CommentServices>();
            services.AddScoped<IGenreServices, GenreServices>();
            services.AddScoped<IAuthHelper, AuthHelper>();
            services.AddScoped<IEmailService, EmailService>();

            services.Configure<SMTPConfigModel>(Configuration.GetSection("SMTPConfig"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MovieForum V1");
            });

            app.UseRouting();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}


