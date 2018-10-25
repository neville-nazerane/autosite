using AutoSite.Core.Models;
using AutoSite.Services;
using NetCore.Apis.Consumer;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AutoSite.Business.Extensions
{
    class EntityUnderstanding : IEntityUnderstanding
    {
        private readonly ApiConsumer consumer;
        private readonly Options options;

        public EntityUnderstanding(HttpClient client, Options options)
        {
            consumer = new ApiConsumer(client);
            this.options = options;
        }

        public async Task<LuisResult> GetAsync(string query)
        {
            var result = await consumer.GetAsync<LuisResult>($"{options.Path}?q={query}");
            if (result.IsSuccessful) return result;
            else throw new HttpRequestException($"{result.StatusCode}: '{result.TextResponse}'");
        }

        internal class Options
        {
            public string Path { get; set; }
        }

    }
}
