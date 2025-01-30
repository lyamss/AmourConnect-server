using Microsoft.EntityFrameworkCore;
using API.Services;
using API.Entities;
using API.Persistence;


namespace API.Features.RequestFriend
{
    internal sealed class RequestFriendsRepository : Repository<RequestFriends> ,IRequestFriendsRepository
    {
        public RequestFriendsRepository(BackendDbContext _context) : base(_context) {}
        public async Task<ICollection<QueryRequestFriend>> GetRequestFriendsAsync(int Id_User, CancellationToken cancellationToken) =>
            await this.context.RequestFriends
                .Include(r => r.UserIssuer)
                .Include(r => r.UserReceiver)
                .Where(r => r.IdUserIssuer == Id_User || r.Id_UserReceiver == Id_User)
                .AsSplitQuery()
                .Select(r => r.ToGetRequestFriendsMapper())
                .ToListAsync(cancellationToken);

        public async Task<RequestFriendForGetMessageDto> GetRequestFriendByIdAsync(int IdUserIssuer, int IdUserReceiver, CancellationToken cancellationToken) =>
            await this.context.RequestFriends
                    .Where(r => (r.IdUserIssuer == IdUserIssuer && r.Id_UserReceiver == IdUserReceiver)
                        || (r.IdUserIssuer == IdUserReceiver && r.Id_UserReceiver == IdUserIssuer))
                        .Select(r => r.ToGetRequestFriendsForGetMessageMapper())
                        .FirstOrDefaultAsync(cancellationToken);

        public async Task<RequestFriends> GetUserFriendRequestByIdAsync(int Id_User, int IdUserIssuer, CancellationToken cancellationToken) =>
            await this.context.RequestFriends
        .Include(r => r.UserIssuer)
        .Include(r => r.UserReceiver)
        .AsSplitQuery()
        .FirstOrDefaultAsync(r =>
            r.IdUserIssuer == IdUserIssuer && r.Id_UserReceiver == Id_User && r.Status == RequestStatus.Onhold, cancellationToken);
    }


    public interface IRequestFriendsRepository
    {
        Task<ICollection<QueryRequestFriend>> GetRequestFriendsAsync(int Id_User, CancellationToken cancellationToken);
        Task<RequestFriendForGetMessageDto> GetRequestFriendByIdAsync(int IdUserIssuer, int IdUserReceiver, CancellationToken cancellationToken);
        Task<RequestFriends> GetUserFriendRequestByIdAsync(int Id_User, int IdUserIssuer, CancellationToken cancellationToken);
    }
}