using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SampleLambdaFunction
{
    public class Function
    {
        
        /// <summary>
        /// A simple function that accepts a city name, and queries weather data for that city.
        /// </summary>
        /// <param name="input">a city name</param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<string> FunctionHandler(string input, ILambdaContext context)
        {
            context.Logger.LogLine("Querying OpenWeather for: " + input);
            var weatherKey = Environment.GetEnvironmentVariable("WEATHER_API_KEY");
            var query = $"https://api.openweathermap.org/data/2.5/weather?q={input}&appid={weatherKey}";
            using (var http = new HttpClient())
            {
                var output = await http.GetStringAsync(query);
                return output;
            }
        }
    }
}
