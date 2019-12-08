using HellsGate.Controllers;
using Xunit;

namespace XUnitTest.TestWebApi
{
    public class PlateVerificationTest
    {
        private PlateVerificationController _test;

        [Theory]
        [InlineData("AA123BB")]
        public void AdminPlate(string value)
        {
            Assert.True(_test.Get(value).Result);
        }
    }
}