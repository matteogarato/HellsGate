using Xunit;

namespace XUnitTest.TestWebApi
{
    public class PlateVerificationTest
    {
        [Theory]
        [InlineData("AA123BB")]
        public void AdminPlate(string value)
        {
            //PlateVerificationController _test = new PlateVerificationController();
            //Assert.True(_test.Get(value).Result);
        }
    }
}