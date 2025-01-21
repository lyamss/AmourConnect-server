using Domain.Dtos.SetDtos;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Services
{
    public interface IRegexUtils
    {
        (bool success, string message) CheckBodyAuthRegister(SetUserRegistrationDto setUserRegistrationDto);
        bool CheckPicture(IFormFile Profile_picture);
        bool CheckCity(string city);
        bool CheckSex(string sex);
        bool CheckDate(DateTime? date);
        bool CheckPseudo(string Pseudo);
        bool CheckDescription(string Description);
        bool CheckMessage(string Message);
    }
}