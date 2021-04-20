using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;

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

            // (!) Injecting strings into your query without checking them first is dangerous. Don't do it in production!
            var query = $"https://api.openweathermap.org/data/2.5/weather?q={input}&appid={weatherKey}";
            using (var http = new HttpClient())
            {
                var output = await http.GetStringAsync(query);
                var bucket = "sample-lambda-storage";
                var key = $"weather_{DateTime.Now.Ticks}.json";
                var stored = await PutToS3Async(bucket, key, output, context);
                var msg = stored
                    ? $"Successfully stored weather data: {bucket}:{key}"
                    : $"Failed to store weather data.";
                return msg;
            }
        }

        private async Task<bool> PutToS3Async(string bucket, string key, string content, ILambdaContext context)
        {
            var request = new PutObjectRequest
            {
                BucketName = bucket,
                Key = key,
                ContentBody = content
            };

            using (var s3 = new AmazonS3Client(RegionEndpoint.EUWest2))
            {
                var response = await s3.PutObjectAsync(request);
                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    context.Logger.Log($"{response.HttpStatusCode} encountered putting: {bucket}:{key}");
                    return false;
                }

            }

        }
    }
}
