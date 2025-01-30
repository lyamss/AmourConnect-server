using API.Entities;
using API.Services;
using MediatR;

namespace API.Features.Users.UpdateUser;

internal sealed class Handler : IRequestHandler<CommandUpdateUser, ApiResponseDto>
{

    private readonly DataUser dataUser;
    private readonly IRegexUtils _regexUtils;
    private readonly IMessUtils _messUtils;
    private readonly IRepository<User> _repositoryU; 
    public Handler(DataUser _dataUser)
    {
        this.dataUser = _dataUser;
    }

    public async Task<ApiResponseDto> Handle(CommandUpdateUser commandUpdateUser, CancellationToken cancellationToken)
    {
        User dataUserNowConnect = await this.dataUser.GetDataUserConnected(cancellationToken);

            var newsValues = this.UpdatingCheckUser(commandUpdateUser, await this._messUtils.ConvertImageToByteArrayAsync(commandUpdateUser.Profile_picture), dataUserNowConnect);

            dataUserNowConnect.Profile_picture = newsValues.Profile_picture;
            dataUserNowConnect.city = newsValues.city;
            dataUserNowConnect.sex = newsValues.sex;
            dataUserNowConnect.Description = newsValues.Description;
            dataUserNowConnect.date_of_birth = newsValues.date_of_birth;

            await this._repositoryU.SaveChangesAsync(cancellationToken);

            QueryUser UserDtoNewValues = newsValues.ToGetUserMapper();

            return ApiResponseDto.Success("yes good", UserDtoNewValues);
    }


     private User UpdatingCheckUser(CommandUpdateUser setUserUpdateDto, byte[] imageData, User dataUserNowConnect)
    {
        return new User
        {
                Profile_picture = this._regexUtils.CheckPicture(setUserUpdateDto.Profile_picture)
                ? imageData: dataUserNowConnect.Profile_picture,

                city = this._regexUtils.CheckCity(setUserUpdateDto.city) 
                ? setUserUpdateDto.city : dataUserNowConnect.city,

                Description = this._regexUtils.CheckDescription(setUserUpdateDto.Description)
                ? setUserUpdateDto.Description : dataUserNowConnect.Description,

                sex = this._regexUtils.CheckSex(setUserUpdateDto.sex)
                ? setUserUpdateDto.sex : dataUserNowConnect.sex,

                date_of_birth = this._regexUtils.CheckDate(setUserUpdateDto.date_of_birth)
                ? setUserUpdateDto.date_of_birth ?? DateTime.MinValue : dataUserNowConnect.date_of_birth,
        };
    }
}