using API.Entities;
using API.Services;
using MediatR;

namespace API.Features.Users.GetUserId;

internal sealed class Handler : IRequestHandler<CommandGetUserId, ApiResponseDto>
{
    private readonly IRepository<User> repositoryU;

    public Handler(IRepository<User> _repositoryU)
    {
        this.repositoryU = _repositoryU;
    }

    public async Task<ApiResponseDto> Handle(CommandGetUserId commandGetUserId, CancellationToken cancellationToken)
    {
        User user = await this.repositoryU.GetByIdAsync(commandGetUserId.Id_User, cancellationToken);

        if (user == null) 
            return ApiResponseDto.Failure("no found :/");

        QueryUser userDto = user.ToGetUserMapper();

        return ApiResponseDto.Success("found", userDto);
    }
}