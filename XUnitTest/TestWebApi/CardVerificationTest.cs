using Xunit;

namespace XUnitTest.TestWebApi
{
    public class CardVerificationTest
    {
        [Theory]
        [InlineData("AA123BB")]
        public void AdminCardShouldBeOK(string value)
        {
            //CardVerificationController _test = new CardVerificationController();
            //Assert.True(_test.Get(value).Result);
        }

        [Theory]
        [InlineData("AA123BB")]
        public void AdminCardShouldBeKO(string value)
        {
            //CardVerificationController _test = new CardVerificationController();
            //Assert.True(_test.Get(value).Result);
        }
    }
}