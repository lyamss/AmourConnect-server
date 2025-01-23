using Application.Services;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Tests.Application.ServicesTests.RegexTests
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

        [Fact]
        public void CheckPicture_ReturnsFalse_WhenProfilePictureLengthIsZero()
        {
            var profilePicture = new Mock<IFormFile>();
            profilePicture.SetupGet(p => p.Length).Returns(0);

            bool result = RegexUtils.CheckPicture(profilePicture.Object);

            Assert.False(result);
        }

        [Fact]
        public void CheckPicture_ReturnsFalse_WhenProfilePictureSizeExceedsMaxSize()
        {
            var profilePicture = new Mock<IFormFile>();
            profilePicture.SetupGet(p => p.Length).Returns(1024 * 1024 + 1);

            bool result = RegexUtils.CheckPicture(profilePicture.Object);

            Assert.False(result);
        }

        [Theory]
        [InlineData("application/pdf")]
        [InlineData("image/tiff")]
        [InlineData("image/bmp")]
        public void CheckPicture_ReturnsFalse_WhenProfilePictureTypeIsNotAllowed(string contentType)
        {
            var profilePicture = new Mock<IFormFile>();
            profilePicture.SetupGet(p => p.ContentType).Returns(contentType);

            bool result = RegexUtils.CheckPicture(profilePicture.Object);

            Assert.False(result);
        }

        [Theory]
        [InlineData("image/png")]
        [InlineData("image/jpeg")]
        [InlineData("image/gif")]
        public void CheckPicture_ReturnsTrue_WhenProfilePictureIsValid(string contentType)
        {
            var profilePicture = new Mock<IFormFile>();
            profilePicture.SetupGet(p => p.Length).Returns(1024 * 1024 * 1);
            profilePicture.SetupGet(p => p.ContentType).Returns(contentType);

            bool result = RegexUtils.CheckPicture(profilePicture.Object);

            Assert.True(result);
        }
    }
}