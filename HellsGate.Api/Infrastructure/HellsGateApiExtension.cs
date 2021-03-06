﻿using HellsGate.Api.Infrastructure;
using HellsGate.Models.Context;
using HellsGate.Models.DatabaseModel;
using HellsGate.Services;
using HellsGate.Services.Interfaces;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace HellsGate.Infrastructure
{
    public static class HellsGateApiExtension
    {
        public static void AddHellsGateApi(this IServiceCollection services, IConfiguration Configuration)
        {
            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            //var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = appSettings.Issuer,
                    ValidAudience = appSettings.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Secret))
                };
            });
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });
            services.AddDbContext<HellsGateContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("HellsGateContext")));

            services.AddIdentity<PersonModel, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<HellsGateContext>()
            .AddDefaultTokenProviders();

            services.AddScoped<IAccessManagerService, AccessManagerService>();
            services.AddScoped<IAutorizationManagerService, AutorizationManagerService>();
            services.AddScoped<INodeService, NodeService>();
            services.AddScoped<ILoginManagerService, LoginManagerService>();
            services.AddScoped<SignInManager<PersonModel>, LoginManagerService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<ISecurLibService, SecurLibService>();

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }
    }
}