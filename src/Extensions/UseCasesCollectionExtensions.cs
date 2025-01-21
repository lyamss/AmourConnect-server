using Application.Interfaces.Filters;
using Application.UseCases.Filters;
using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces.Controllers;
using Application.UseCases.Controllers;
namespace Application.Extensions
{
    public static class UseCasesCollectionExtensions
    {
        public static void AddCaseControllers(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizeAuthUseCase, AuthorizeAuthUseCase>();
            services.AddScoped<IAuthUseCase, AuthUseCase>();
            services.AddScoped<IUserUseCase, UserUseCase>();
            services.AddScoped<IMessageUseCase, MessageUseCase>();
            services.AddScoped<IRequestFriendsUseCase, RequestFriendsUseCase>();
        }
    }
}