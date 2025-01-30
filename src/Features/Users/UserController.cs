using API.Features.Authentification.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace API.Features.Users
{
    [Route("api/v1/[controller]")]
    [ServiceFilter(typeof(AuthorizeAuth))]
    public partial class UserController
    (
    IMediator _mediator
    ) 
    : ControllerBase
    {
        private readonly IMediator mediator = _mediator;
    }
}