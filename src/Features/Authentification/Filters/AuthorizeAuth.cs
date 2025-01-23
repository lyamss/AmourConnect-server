using Microsoft.AspNetCore.Mvc.Filters;


namespace API.Features.Authentification.Filters
{
    internal sealed class AuthorizeAuth(IAuthorizeAuthUseCase authorizeAuthUseCase) : Attribute, IAsyncAuthorizationFilter
    {
        private readonly IAuthorizeAuthUseCase _authorizeAuthUseCase = authorizeAuthUseCase;

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context) => await this._authorizeAuthUseCase.OnAuthorizationAsync(context);
    }
}