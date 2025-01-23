using API.Services;

namespace Tests.ServicesTests.RegexTests
{
    public class CheckDescription
    {
        private static readonly RegexUtils RegexUtils = new();

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("M", true)]
        [InlineData("AB", true)]
        [InlineData("MFzehbd", true)]
        [InlineData("Fsapeodjsmzpdje", true)]
        [InlineData("Mze@?n", true)]
        [InlineData("Fsapeodjsmzpdjes", true)]
        [InlineData("M#", true)]
        [InlineData("Intheheartofthecityaloneviolinist'smelancholictuneechoedcaptivatingthesilentheartsofpassersby", true)]
        [InlineData("In the heart of the city, a lone violinist's melancholic tune echoed, captivating the silent hearts of passersby. Lol", false)]
        public void ShouldReturnExpectedResult(string description, bool expected)
        {
            bool result = RegexUtils.CheckDescription(description);

            Assert.Equal(expected, result);
        }
    }
}