using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace AutoSite.Website
{
    public static class WebHostExtensions
    {

        const string urlsKey = "USING_URLS";
        const string certificatePath = "CERTIFICATE_PATH";

        public static IWebHostBuilder UseUrlsIfProvided(this IWebHostBuilder builder)
        {
            var urls = Environment.GetEnvironmentVariable(urlsKey)?.Split(";");
            if (urls != null) builder.UseUrls(urls);
            return builder;
        }
        
    }
}
