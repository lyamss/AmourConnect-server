namespace Application.Interfaces.Controllers
{
    public interface IRequestFriendsUseCase
    {
        Task GetRequestFriendsAsync();
        Task AcceptFriendRequestAsync(int IdUserIssuer);
        Task AddRequestFriendsAsync(int IdUserReceiver);
    }
}