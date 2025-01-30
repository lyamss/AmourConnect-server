using API.Features.Authentification.Filters;
using API.Features.Authentification.Register;
using API.Features.Message;
using API.Features.RequestFriend;
using API.Features.Users;
using API.Persistence;
using API.Seeders;
using API.Services;
using API.Services.Email;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, string ConnexionDB, string ConnexionRedis)
        {
            services.AddDbContext<BackendDbContext>(options => 
            {
                options.UseNpgsql(ConnexionDB);
            });

            services.AddStackExchangeRedisCache(rediosOptions =>
            {
                rediosOptions.Configuration = (ConnexionRedis);
            });

            services.AddScoped<IUserSeeder, UserSeeder>();
            services.AddScoped<IBackendDbContext, BackendDbContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IRequestFriendsRepository, RequestFriendsRepository>();
            services.AddScoped<AuthorizeAuth>();
            services.AddScoped<IAuthorizeAuthUseCase, AuthorizeAuthUseCase>();
            services.AddScoped<IJWTSessionUtils, JWTSessionUtils>();
            services.AddScoped<IRegexUtils, RegexUtils>();
            services.AddSingleton<SecretEnv>();
            services.AddScoped<ISendMail, SendMail>();
            services.AddScoped<IConfigEmail, ConfigEmail>();
            services.AddScoped<IBodyEmail, BodyEmail>();
            services.AddScoped<IMessUtils, MessUtils>();
            services.AddScoped<DataUser>();
            services.AddSingleton<StringConfig>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(RegisterHandler).Assembly);
            });
        }
    }
}