using Domain.Dtos.AppLayerDtos;
using Domain.Dtos.GetDtos;

namespace Infrastructure.Interfaces
{
    public interface IRequestFriendsCaching
    {
        Task<ICollection<GetRequestFriendsDto>> GetRequestFriendsAsync(int Id_User);
        Task<RequestFriendForGetMessageDto> GetRequestFriendByIdAsync(int IdUserIssuer, int IdUserReceiver);
    }
}