using Domain.Dtos.GetDtos;
using Domain.Entities;

namespace Infrastructure.Interfaces
{
    public interface IUserCaching
    {
        Task<User> GetUserWithCookieAsync(string token_session_user);
        Task<ICollection<GetUserDto>> GetUsersToMatchAsync(User dataUserNowConnect);
        Task<User> GetUserByIdUserAsync(int Id_User);
    }
}