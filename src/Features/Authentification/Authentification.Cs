using API.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace API.Features.Authentification;



[Route("api/v1/[controller]")]
public partial class Authentification : ControllerBase
{
    private readonly SecretEnv secretEnv;
    private readonly IMediator mediator;

    public Authentification
    (
        SecretEnv secretEnv,
        IMediator mediator
    )
    {
        this.secretEnv = secretEnv;
        this.mediator = mediator;
    }
}