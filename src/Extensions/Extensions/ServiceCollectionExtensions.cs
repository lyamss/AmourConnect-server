using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Seeders;
using Infrastructure.Interfaces;
using Infrastructure.Repository;
using Infrastructure.DistributedCaching;

namespace Infrastructure.Extensions
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
            services.AddScoped<IUserCaching, UserCaching>();
            services.AddScoped<IRequestFriendsCaching, RequestFriendsCaching>();
            services.AddScoped<IMessageCaching, MessageCaching>();
            services.AddTransient<ICacheService, CacheService>();
        }
    }
}