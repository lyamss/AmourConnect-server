
using API.Services;

namespace Tests.ServicesTests.RegexTests
{
    public class CheckCity
    {

        private static readonly RegexUtils regexUtils = new();

        [Theory]
        [InlineData("sjehbzjvefjzefvzehfzveehgfzh", true)]
        [InlineData("sjehbz", true)]
        [InlineData("sjehbzjvefjzefvzehfzveehgfzhzuegfzjefjzevfhgzevfhzvedhgzvedhzvehgvqzeghvzjefvjgqgezvfjvqzejfvqzejfvjqzevfhjqvezhjfvqzhefvj", false)]
        [InlineData("", false)]
        [InlineData("s", false)]
        [InlineData("s@zegbv#?e", false)]
        [InlineData("2434dszhd42", false)]
        [InlineData("ù%$^zdhjz-_", false)]
        [InlineData(null, false)]
        public void ShouldReturnFalse(string city, bool expected)
        {
            bool result = regexUtils.CheckCity(city);

            Assert.Equal(expected, result);
        }
    }
}