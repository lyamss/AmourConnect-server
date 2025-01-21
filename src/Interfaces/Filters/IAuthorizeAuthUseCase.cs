using Microsoft.AspNetCore.Mvc.Filters;

namespace Application.Interfaces.Filters
{
    public interface IAuthorizeAuthUseCase
    {
        Task OnAuthorizationAsync(AuthorizationFilterContext context);
    }
}