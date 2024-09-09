﻿using AmourConnect.App.Interfaces.Services;
using AmourConnect.App.Services;
using Microsoft.Extensions.DependencyInjection;
using AmourConnect.App.Services.Email;
using AmourConnect.App.Interfaces.Services.Email;
using AmourConnect.Domain.Utils;
using Microsoft.Extensions.Options;
namespace AmourConnect.App.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static void AddServicesControllers(this IServiceCollection services)
        {
            services.AddScoped<IRegexUtils, RegexUtils>();
            services.AddScoped<ISendMail, SendMail>();
            services.AddScoped<IMessUtils, MessUtils>();
            services.AddScoped<IConfigEmail, ConfigEmail>();
            services.AddScoped<IJWTSessionUtils, JWTSessionUtils>();
            services.AddScoped<IJwtSecret, JwtSecret>();
        }
    }
}