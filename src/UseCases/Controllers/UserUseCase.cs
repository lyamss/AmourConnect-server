using Application.Interfaces.Controllers;
using Domain.Dtos.GetDtos;
using Domain.Entities;
using Domain.Dtos.SetDtos;
using Infrastructure.Interfaces;
using Domain.Mappers;
using Microsoft.AspNetCore.Http;
using Application.Interfaces.Services;
using Application.Services;

namespace Application.UseCases.Controllers
{
    internal sealed class UserUseCase(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IRegexUtils regexUtils, IMessUtils messUtils, IJWTSessionUtils jWTSessionUtils, IUserCaching userCaching) : IUserUseCase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly string token_session_user = jWTSessionUtils.GetValueClaimsCookieUser(httpContextAccessor.HttpContext);
        private readonly IRegexUtils _regexUtils = regexUtils;
        private readonly IMessUtils _messUtils = messUtils;
        private readonly IUserCaching _userCaching = userCaching;


        public async Task GetUsersToMach()
        {
            User dataUserNowConnect = await _GetDataUserConnected(token_session_user);

            ICollection<GetUserDto> users = await _userCaching.GetUsersToMatchAsync(dataUserNowConnect);

            throw new ExceptionAPI(true, "yes good", users );
        }

        public async Task GetUserConnected()
        {
            User dataUserNowConnect = await _GetDataUserConnected(token_session_user);

            GetUserDto userDto = dataUserNowConnect.ToGetUserMapper();

            throw new ExceptionAPI(true, "yes good", userDto);
        }

        public async Task UpdateUser(SetUserUpdateDto setUserUpdateDto)
        {
            User dataUserNowConnect = await _GetDataUserConnected(token_session_user);

            var newsValues = UpdatingCheckUser(setUserUpdateDto, await _messUtils.ConvertImageToByteArrayAsync(setUserUpdateDto.Profile_picture), dataUserNowConnect);

            dataUserNowConnect.Profile_picture = newsValues.Profile_picture;
            dataUserNowConnect.city = newsValues.city;
            dataUserNowConnect.sex = newsValues.sex;
            dataUserNowConnect.Description = newsValues.Description;
            dataUserNowConnect.date_of_birth = newsValues.date_of_birth;

            await _userRepository.UpdateUserAsync(dataUserNowConnect.Id_User, dataUserNowConnect);

            GetUserDto UserDtoNewValues = newsValues.ToGetUserMapper();

            throw new ExceptionAPI(true, "yes good", UserDtoNewValues);
        }

        public async Task GetUserById(int Id_User)
        {
            User user = await _userCaching.GetUserByIdUserAsync(Id_User);

            if (user == null) 
                throw new ExceptionAPI(false, "no found :/", null);

            GetUserDto userDto = user.ToGetUserMapper();

            throw new ExceptionAPI(true, "found", userDto);
        }

        private User UpdatingCheckUser(SetUserUpdateDto setUserUpdateDto, byte[] imageData, User dataUserNowConnect)
        {
            return new User
            {
                   Profile_picture = _regexUtils.CheckPicture(setUserUpdateDto.Profile_picture)
                    ? imageData: dataUserNowConnect.Profile_picture,

                   city = _regexUtils.CheckCity(setUserUpdateDto.city) 
                   ? setUserUpdateDto.city : dataUserNowConnect.city,

                   Description = _regexUtils.CheckDescription(setUserUpdateDto.Description)
                   ? setUserUpdateDto.Description : dataUserNowConnect.Description,

                   sex = _regexUtils.CheckSex(setUserUpdateDto.sex)
                    ? setUserUpdateDto.sex : dataUserNowConnect.sex,

                   date_of_birth = _regexUtils.CheckDate(setUserUpdateDto.date_of_birth)
                   ? setUserUpdateDto.date_of_birth ?? DateTime.MinValue : dataUserNowConnect.date_of_birth,
            };
        }
        private async Task<User> _GetDataUserConnected(string token_session_user) => await _userCaching.GetUserWithCookieAsync(token_session_user);
    }
}