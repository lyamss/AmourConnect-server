using API.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;


namespace API.Features.Authentification;



[Route("api/v1/[controller]")]
public partial class Authentification : ControllerBase
{
    private readonly IOptions<SecretEnv> secretEnv;
    private readonly IMediator mediator;

    public Authentification
    (
        IOptions<SecretEnv> secretEnv,
        IMediator mediator
    )
    {
        this.secretEnv = secretEnv;
        this.mediator = mediator;
    }
}