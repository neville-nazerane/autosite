using AutoSite.Core.Models;
using AutoSite.Services;
using Microsoft.AspNetCore.Http;
using NetCore.Apis.Consumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AutoSite.Business
{
    class CustomVisionReader : ICustomVisionReader
    {
        private readonly HttpClient client;
        private readonly Options options;

        public CustomVisionReader(HttpClient client, Options options)
        {
            this.client = client;
            this.options = options;
        }

        public async Task<string> GetPrediction(IFormFile image)
        {
            var content = new StreamContent(image.OpenReadStream());
            content.Headers.Add("Content-Type", "application/octet-stream");
            ApiConsumedResponse<CustomVisionResult> result = await client.PostAsync(options.Path, content);
            if (result.IsSuccessful)
            {
                var predictions = result.Data.Predictions;
                return predictions.SingleOrDefault(p => p.Probability == predictions.Max(m => m.Probability)).TagName;
            }
            else throw new HttpRequestException($"{result.StatusCode}: {result.TextResponse}");
        }

        internal class Options
        {
            public string Path { get; set; }
        }

    }
}
