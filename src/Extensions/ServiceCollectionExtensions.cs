using API.Features.Message;
using API.Features.RequestFriend;
using API.Features.Users;
using API.Persistence;
using API.Seeders;
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
        }
    }
}