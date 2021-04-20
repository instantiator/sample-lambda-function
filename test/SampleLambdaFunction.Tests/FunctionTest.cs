using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

using SampleLambdaFunction;

namespace SampleLambdaFunction.Tests
{
    public class FunctionTest
    {
        [Fact]
        public async Task TestWeatherReturnsData()
        {
            var function = new Function();
            var context = new TestLambdaContext();
            var weather = await function.FunctionHandler("London", context);
            Assert.False(string.IsNullOrWhiteSpace(weather));
        }
    }
}
