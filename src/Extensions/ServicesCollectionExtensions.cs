using Application.Interfaces.Services;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Application.Services.Email;
using Application.Interfaces.Services.Email;
using Domain.Utils;
namespace Application.Extensions
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
            services.AddScoped<ISecretEnv, SecretEnv>();
            services.AddScoped<IBodyEmail, BodyEmail>();
        }
    }
}