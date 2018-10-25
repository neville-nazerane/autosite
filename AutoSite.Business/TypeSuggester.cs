using AutoSite.Core.Models;
using AutoSite.Services;
using NetCore.Apis.Consumer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AutoSite.Business
{
    class TypeSuggester : ITypeSuggester
    {

        readonly ApiConsumer consumer;
        private readonly Options options;

        public TypeSuggester(HttpClient client, Options options)
        {
            consumer = new ApiConsumer(client);
            this.options = options;
        }

        public async Task<string> SuggestAsync(string name)
        {
            var res = (await SuggestRawAsync(name)).Answers;
            return res.SingleOrDefault(
                    r => r.Score == res.Max(m => m.Score))?.Answer ?? "invalid";
        }

        public async Task<QnAResponse> SuggestRawAsync(string name)
        {
            ApiConsumedResponse<QnAResponse> response = await consumer.PostAsync(options.Path, new DataContent
            {
                Question = name
            });
            if (response.IsSuccessful) return response;
            else throw new HttpRequestException(response.TextResponse);
        }
        
        class DataContent
        {
            public string Question { get; set; }
        }

        internal class Options
        {
            public string Path { get; set; }
        }
    }
}
