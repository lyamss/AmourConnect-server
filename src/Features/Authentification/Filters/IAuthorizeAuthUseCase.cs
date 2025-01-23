using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Features.Authentification.Filters
{
    internal interface IAuthorizeAuthUseCase
    {
        Task OnAuthorizationAsync(AuthorizationFilterContext context);
    }
}