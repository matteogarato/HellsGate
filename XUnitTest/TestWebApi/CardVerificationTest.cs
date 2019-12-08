using HellsGate.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTest.TestWebApi
{
    public class CardVerificationTest
    {
        private CardVerificationController _test;

        [Theory]
        [InlineData("AA123BB")]
        public void AdminCard(string value)
        {
            Assert.True(_test.Get(value).Result);
        }
    }
}