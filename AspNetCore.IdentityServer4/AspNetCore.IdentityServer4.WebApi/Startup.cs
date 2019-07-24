﻿using System;
using AspNetCore.IdentityServer4.WebApi.Models;
using AspNetCore.IdentityServer4.WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.IdentityServer4.WebApi
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Enable Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = "https://localhost:6001";
                options.RequireHttpsMetadata = true;
                options.Audience = "MyBackend";
                options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
            });

            // Inject AppSetting configuration
            services.Configure<AppSettings>(this.Configuration);

            // Inject HttpClient
            services.AddHttpClient<IAuthService, AuthService>().SetHandlerLifetime(TimeSpan.FromMinutes(2)); // HttpMessageHandler lifetime = 2 min

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Authentication
            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
