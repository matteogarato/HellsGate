using HellsGate.Controllers;
using Xunit;

namespace XUnitTest.TestWebApi
{
    public class CardVerificationTest
    {
        [Theory]
        [InlineData("AA123BB")]
        public void AdminCard(string value)
        {
            CardVerificationController _test = new CardVerificationController();
            Assert.True(_test.Get(value).Result);
        }
    }
}