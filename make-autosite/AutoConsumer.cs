using AutoSite.Core.Entities;
using NetCore.Apis.Consumer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace make_autosite
{
    static class AutoConsumer
    {

#if RELEASE
        const string apiBase = "http://autosite.nevillenazerane.com";
#else
        const string apiBase = "http://localhost:52612";
#endif


        const string sitePath = "api/sites";

        static readonly ApiConsumer consumer = new ApiConsumer(apiBase);

        static internal async Task<ApiConsumedResponse<SiteContent>> GetSiteAsync(string name)
            => await consumer.GetAsync($"{sitePath}/{name}");

    }
}
