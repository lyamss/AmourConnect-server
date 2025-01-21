using Microsoft.AspNetCore.Mvc.Filters;
using Application.Interfaces.Filters;

namespace API.Filters
{
    internal class AuthorizeAuth(IAuthorizeAuthUseCase authorizeAuthUseCase) : Attribute, IAsyncAuthorizationFilter
    {
        private readonly IAuthorizeAuthUseCase _authorizeAuthUseCase = authorizeAuthUseCase;

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context) => await _authorizeAuthUseCase.OnAuthorizationAsync(context);
    }
}