using API.Services;
using Microsoft.AspNetCore.Http;


namespace Tests.ServicesTests.RegexTests
{
    public class CheckPictures
    {
        private static readonly RegexUtils RegexUtils = new();

        [Fact]
        public void CheckPicture_ReturnsFalse_WhenProfilePictureIsNull()
        {
            IFormFile profilePicture = null;

            bool result = RegexUtils.CheckPicture(profilePicture);

            Assert.False(result);
        }
    }
}