using API.Services;

namespace Tests.ServicesTests.RegexTests
{
    public class CheckDate
    {
        private static readonly RegexUtils regexUtils = new();

        [Theory]
        [InlineData("2000-01-01", true)]
        [InlineData("2000-1-1", true)]
        [InlineData("2000-01-01T00:00:00", true)]
        [InlineData("2005-01-01", true)]
        [InlineData("1900-01-01", false)]
        [InlineData("2100-01-01", false)]
        public void CheckDate_ShouldReturnExpectedResult(string date, bool expected)
        {
            DateTime? dateTime = DateTime.Parse(date);

            bool result = regexUtils.CheckDate(dateTime);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void CheckDate_ShouldReturnNull() 
        {
            DateTime? dateTime = null;

            bool result = regexUtils.CheckDate(dateTime);
            
            Assert.False(result);
        }
    }
}