using Domain.Dtos.GetDtos;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Domain.Mappers;
using Infrastructure.Persistence;
using Domain.Dtos.AppLayerDtos;
namespace Infrastructure.Repository
{
    internal sealed class RequestFriendsRepository(BackendDbContext _context) : IRequestFriendsRepository
    {
        public async Task<ICollection<GetRequestFriendsDto>> GetRequestFriendsAsync(int Id_User) =>
            await _context.RequestFriends
                .Include(r => r.UserIssuer)
                .Include(r => r.UserReceiver)
                .Where(r => r.IdUserIssuer == Id_User || r.Id_UserReceiver == Id_User)
                .AsSplitQuery()
                .Select(r => r.ToGetRequestFriendsMapper())
                .ToListAsync();

        public async Task<RequestFriendForGetMessageDto> GetRequestFriendByIdAsync(int IdUserIssuer, int IdUserReceiver) =>
            await _context.RequestFriends
                    .Where(r => (r.IdUserIssuer == IdUserIssuer && r.Id_UserReceiver == IdUserReceiver)
                        || (r.IdUserIssuer == IdUserReceiver && r.Id_UserReceiver == IdUserIssuer))
                        .Select(r => r.ToGetRequestFriendsForGetMessageMapper())
                        .FirstOrDefaultAsync();


        public async Task AddRequestFriendAsync(RequestFriends requestFriends)
        {
            await _context.RequestFriends.AddAsync(requestFriends);
            await _context.SaveChangesAsync();
        }


        public async Task<RequestFriends> GetUserFriendRequestByIdAsync(int Id_User, int IdUserIssuer) =>
            await _context.RequestFriends
        .Include(r => r.UserIssuer)
        .Include(r => r.UserReceiver)
        .AsSplitQuery()
        .FirstOrDefaultAsync(r =>
            (r.IdUserIssuer == IdUserIssuer && r.Id_UserReceiver == Id_User && r.Status == RequestStatus.Onhold));



        public async Task UpdateStatusRequestFriendsAsync(RequestFriends friendRequest)
        {
            _context.Entry(friendRequest).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}