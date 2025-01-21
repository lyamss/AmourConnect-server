using Domain.Dtos.SetDtos;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Application.Interfaces.Services;
namespace Application.Services
{
    public class RegexUtils : IRegexUtils
    {
        private readonly Regex DateRegex = new(@"^\d{4}-\d{2}-\d{2}$", RegexOptions.Compiled); // (format : YYYY-MM-DD)
        private readonly Regex CityRegex = new(@"^[a-zA-Z\s]{2,50}$", RegexOptions.Compiled);
        private readonly Regex PseudoRegex = new(@"^[a-zA-Z0-9_]{1,15}$", RegexOptions.Compiled);
        private readonly Regex MessageRegex = new(@"^.{1,200}$", RegexOptions.Compiled);
        private readonly Regex DescriptionRegex = new(@"^.{1,100}$", RegexOptions.Compiled);

        public (bool success, string message) CheckBodyAuthRegister(SetUserRegistrationDto setUserRegistrationDto)
        {
            if (!CheckDate(setUserRegistrationDto.date_of_birth))
                return (false, "Invalid date of birth format or length");

            if (!CheckSex(setUserRegistrationDto.sex))
                return (false, "Invalid sex value or length");

            if (!CheckCity(setUserRegistrationDto.city))
                return (false, "Invalid city or length");

            if (!CheckPseudo(setUserRegistrationDto.Pseudo))
                return (false, "Invalid pseudo or length");

            if (!CheckDescription(setUserRegistrationDto.Description))
                return (false, "Invalid description or length");

            return (true, string.Empty);
        }



        public bool CheckPicture(IFormFile Profile_picture)
        {

            if (Profile_picture == null || Profile_picture.Length == 0)
                return false;

            const int maxSize = 1 * 1024 * 1024; // 1 Mo

            if (Profile_picture.Length > maxSize)
                return false;

            var allowedTypes = new[] { "image/png", "image/jpeg", "image/gif" };

            if (!allowedTypes.Contains(Profile_picture.ContentType))
                return false;

            return true;
        }



        public bool CheckCity(string city)
        {
            if (string.IsNullOrEmpty(city))
                return false;

            if (!CityRegex.IsMatch(city))
                return false;

            return true;
        }

        public bool CheckSex(string sex)
        {
            if (string.IsNullOrEmpty(sex))
                return false;

            if (!new[] { "M", "F" }.Contains(sex, StringComparer.OrdinalIgnoreCase))
                return false;

            return true;
        }



        public bool CheckDate(DateTime? date)
        {
            if (!date.HasValue || date == null)
                return false;

            string dateString = date.Value.ToString("yyyy-MM-dd");

            if (string.IsNullOrEmpty(dateString))
                return false;

            if (!DateRegex.IsMatch(dateString))
                return false;

            if (date.Value > DateTime.Now)
                return false;

            if (date.Value > DateTime.Now.AddYears(-18))
                return false;

            if (date.Value < DateTime.Now.AddYears(-70))
                return false;

            return true;
        }



        public bool CheckPseudo(string Pseudo)
        {
            if (string.IsNullOrEmpty(Pseudo))
                return false;

            if (!PseudoRegex.IsMatch(Pseudo))
                return false;

            return true;
        }

        public bool CheckDescription(string Description)
        {
            if (string.IsNullOrEmpty(Description))
                return false;

            if (!DescriptionRegex.IsMatch(Description))
                return false;

            return true;
        }


        public bool CheckMessage(string Message)
        {
            if (string.IsNullOrEmpty(Message))
                return false;

            if (!MessageRegex.IsMatch(Message))
                return false;

            return true;
        }
    }
}