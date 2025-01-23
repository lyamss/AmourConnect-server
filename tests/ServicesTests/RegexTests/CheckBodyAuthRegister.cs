using Application.Services;
using Domain.Dtos.SetDtos;

namespace Tests.Application.ServicesTests.RegexTests
{
    public class CheckBodyAuthRegister
    {
            
        private static readonly RegexUtils regexUtils = new();

        public static IEnumerable<object[]> newUserRegistrationTrue =>
            new List<object[]> 
            {
                new object[]  { "ezde", "description", DateTime.Now.AddYears(-18), "F", "ci" },
                new object[]  { "ezdessss", "descriptions", DateTime.Now.AddYears(-19), "M", "citys" },
                new object[]  { "ez", "descriptssion", DateTime.Now.AddYears(-69), "F", "citysd" },
            };

        public static IEnumerable<object[]> newUserRegistrationFalse =>
            new List<object[]>
            {
                        new object[]  { "ezdessssssssssssssssssssssssssssssssssssssss", "description", DateTime.Now.AddYears(-18), "F", "ci" },
                        new object[]  { "ezdessss", "descriptions", DateTime.Now.AddYears(-17), "Ms", "citys" },
                        new object[]  { "ez", "descriptssion", DateTime.Now.AddYears(-71), "Fs", "citysd" },
                        new object[]  { null, null, null, null, null },
            };

        [Theory]
        [MemberData(nameof(newUserRegistrationTrue))]
        public void ShouldReturnNewObjectTrue(string pseudo, string description, DateTime date,string sex, string city)
        {
            SetUserRegistrationDto newUser = new()
            {
                Pseudo = pseudo,
                Description = description,
                date_of_birth = date,
                sex = sex,
                city = city
            };


            var (success, message) = regexUtils.CheckBodyAuthRegister(newUser);

            Assert.True(success);
        }

        [Theory]
        [MemberData(nameof(newUserRegistrationFalse))]
        public void ShouldReturnNewObjectFalse(string pseudo, string description, DateTime date, string sex, string city)
        {
            SetUserRegistrationDto newUser = new()
            {
                Pseudo = pseudo,
                Description = description,
                date_of_birth = date,
                sex = sex,
                city = city
            };

            var (success, message) = regexUtils.CheckBodyAuthRegister(newUser);

            Assert.False(success);
        }
    }
}