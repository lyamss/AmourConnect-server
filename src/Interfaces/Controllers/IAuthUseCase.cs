using Domain.Dtos.SetDtos;

namespace Application.Interfaces.Controllers
{
    public interface IAuthUseCase
    {
        Task ValidateGoogleLoginAsync();
        Task RegisterUserAsync(SetUserRegistrationDto setuserRegistrationDto);
    }
}