using API.Services;


namespace Tests.ServicesTests.RegexTests
{
    public class CheckSex
    {
        private static readonly RegexUtils RegexUtils = new();

        [Theory]
        [InlineData("F", true)]
        [InlineData("M", true)]
        [InlineData("FM", false)]
        [InlineData("MF", false)]
        [InlineData("Fs", false)]
        [InlineData("Ms", false)]
        [InlineData("F@", false)]
        [InlineData("M#", false)]
        [InlineData(null, false)]
        public void CheckSex_ShouldReturnExpectedResult(string sex, bool expected)
        {
            bool result = RegexUtils.CheckSex(sex);

            Assert.Equal(expected, result);
        }
    }
}