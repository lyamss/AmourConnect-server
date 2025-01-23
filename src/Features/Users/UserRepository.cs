using Microsoft.EntityFrameworkCore;
using API.Services;
using API.Persistence;
using API.Entities;


namespace API.Features.Users
{
    internal sealed class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository (BackendDbContext _context) : base(_context) {}
        public async Task<ICollection<QueryUser>> GetUsersToMatchAsync(User dataUserNowConnect, CancellationToken cancellationToken) =>
            await this._context.User
                  .Where(u =>
                      u.city.ToLower() == dataUserNowConnect.city.ToLower() &&
                      u.sex == (dataUserNowConnect.sex == "M" ? "F" : "M") &&
                      u.date_of_birth >= (dataUserNowConnect.sex == "F" ?
                          dataUserNowConnect.date_of_birth.AddYears(-10) :
                          dataUserNowConnect.date_of_birth.AddYears(-1)) &&
                      u.date_of_birth <= (dataUserNowConnect.sex == "M" ?
                          dataUserNowConnect.date_of_birth.AddYears(10) :
                          dataUserNowConnect.date_of_birth.AddYears(1)) &&
                  !this._context.RequestFriends.Any(r =>
                      ((r.IdUserIssuer == u.Id_User && r.Id_UserReceiver == dataUserNowConnect.Id_User) ||
                      (r.Id_UserReceiver == u.Id_User && r.IdUserIssuer == dataUserNowConnect.Id_User)) &&
                      (r.Status == RequestStatus.Accepted || r.Status == RequestStatus.Onhold)))
                  .Select(u => u.ToGetUserMapper())
                  .ToListAsync(cancellationToken);


        public async Task<User> GetUserIdWithGoogleIdAsync(string EmailGoogle, string userIdGoogle, CancellationToken cancellationToken) =>
            await this._context.User
                .Where(u => u.EmailGoogle == EmailGoogle && u.userIdGoogle == userIdGoogle)
                .FirstOrDefaultAsync(cancellationToken);



        public async Task<bool> GetUserByPseudoAsync(string Pseudo, CancellationToken cancellationToken) => await this._context.User.AnyAsync(u => u.Pseudo.ToLower() == Pseudo.ToLower(), cancellationToken);
        
        public async Task<User> GetUserWithCookieAsync(string token_session_user, CancellationToken cancellationToken) => await this._context.User.FirstOrDefaultAsync(u => u.token_session_user == token_session_user, cancellationToken);

        public async Task<User> GetUserByIdUserAsync(int Id_User, CancellationToken cancellationToken) =>
            await this._context.User
                .Where(u => u.Id_User == Id_User)
                .FirstOrDefaultAsync(cancellationToken);
    }



    public interface IUserRepository
    {
        Task<ICollection<QueryUser>> GetUsersToMatchAsync(User dataUserNowConnect, CancellationToken cancellationToken);
        Task<User> GetUserIdWithGoogleIdAsync(string EmailGoogle, string userIdGoogle, CancellationToken cancellationToken);
        Task<bool> GetUserByPseudoAsync(string Pseudo, CancellationToken cancellationToken);
        Task<User> GetUserByIdUserAsync(int Id_User, CancellationToken cancellationToken);
        Task<User> GetUserWithCookieAsync(string token_session_user, CancellationToken cancellationToken);
    }
}