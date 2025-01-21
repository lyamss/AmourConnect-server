using Domain.Dtos.GetDtos;
using Domain.Dtos.AppLayerDtos;
using Domain.Dtos.SetDtos;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Domain.Mappers;
using Infrastructure.Persistence;
namespace Infrastructure.Repository
{
    internal sealed class UserRepository(BackendDbContext _context) : IUserRepository
    {
        public async Task<ICollection<GetUserDto>> GetUsersToMatchAsync(User dataUserNowConnect) =>
                
            await _context.User
                  .Where(u =>
                      u.city.ToLower() == dataUserNowConnect.city.ToLower() &&
                      u.sex == (dataUserNowConnect.sex == "M" ? "F" : "M") &&
                      u.date_of_birth >= (dataUserNowConnect.sex == "F" ?
                          dataUserNowConnect.date_of_birth.AddYears(-10) :
                          dataUserNowConnect.date_of_birth.AddYears(-1)) &&
                      u.date_of_birth <= (dataUserNowConnect.sex == "M" ?
                          dataUserNowConnect.date_of_birth.AddYears(10) :
                          dataUserNowConnect.date_of_birth.AddYears(1)) &&
                  !_context.RequestFriends.Any(r =>
                      ((r.IdUserIssuer == u.Id_User && r.Id_UserReceiver == dataUserNowConnect.Id_User) ||
                      (r.Id_UserReceiver == u.Id_User && r.IdUserIssuer == dataUserNowConnect.Id_User)) &&
                      (r.Status == RequestStatus.Accepted || r.Status == RequestStatus.Onhold)))
                  .Select(u => u.ToGetUserMapper())
                  .ToListAsync();
           




        public async Task<int?> GetUserIdWithGoogleIdAsync(string EmailGoogle, string userIdGoogle) =>
            await _context.User
                .Where(u => u.EmailGoogle == EmailGoogle && u.userIdGoogle == userIdGoogle)
                .Select(u => u.Id_User)
                .FirstOrDefaultAsync();



        public async Task<int?> CreateUserAsync(string userIdGoogle, string EmailGoogle, SetUserRegistrationDto setUserRegistrationDto)
        {
            var user = new User
            {
                userIdGoogle = userIdGoogle,
                EmailGoogle = EmailGoogle,
                Description = setUserRegistrationDto.Description,
                date_of_birth = setUserRegistrationDto.date_of_birth.HasValue ? setUserRegistrationDto.date_of_birth.Value.ToUniversalTime() : DateTime.MinValue,
                sex = setUserRegistrationDto.sex,
                Pseudo = setUserRegistrationDto.Pseudo,
                city = setUserRegistrationDto.city,
                account_created_at = DateTime.Now.ToUniversalTime(),
            };

            await _context.User.AddAsync(user);
            var rowsAffected = _context.SaveChangesAsync();

            if (await rowsAffected > 0)
            {
                return user.Id_User;
            }
            return null;
        }



        public async Task UpdateSessionUserAsync(int Id_User, SessionUserDto JwtGenerate)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Id_User == Id_User);

            if (user is not null)
            {
                user.token_session_user = JwtGenerate.token_session_user;
                user.date_token_session_expiration = JwtGenerate.date_token_session_expiration;

                await _context.SaveChangesAsync();
            }
        }



        public async Task<bool> GetUserByPseudoAsync(string Pseudo) => await _context.User.AnyAsync(u => u.Pseudo.ToLower() == Pseudo.ToLower());
        public async Task<User> GetUserWithCookieAsync(string token_session_user) => await _context.User.FirstOrDefaultAsync(u => u.token_session_user == token_session_user);


        public async Task<bool> UpdateUserAsync(int Id_User, User user)
        {
            User existingUser = await _context.User.FirstOrDefaultAsync(u => u.Id_User == Id_User);

            existingUser.date_of_birth = user.date_of_birth.ToUniversalTime();
            existingUser.sex = user.sex;
            existingUser.Profile_picture = user.Profile_picture;
            existingUser.city = user.city;
            existingUser.Description = user.Description;

            _context.Entry(existingUser).State = EntityState.Modified;
            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0;
        }



        public async Task<User> GetUserByIdUserAsync(int Id_User) =>
            await _context.User
                .Where(u => u.Id_User == Id_User)
                .FirstOrDefaultAsync();
    }
}
