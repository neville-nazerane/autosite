using AutoSite.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoSite.Business.Extensions
{
    public static class ServiceExtensions
    {

        public static IServiceCollection AddAllBusiness(this IServiceCollection services,
                                                IConfiguration configuration)

            => services.AddDbContext<AppDbContext>(config => config.ConfigureDb(configuration))
                .AddTypeMapperClient(configuration)
                .AddEntityUnderstandingClient(configuration)
                .AddVisionClient(configuration)
                .AddCustomVisionClient(configuration)
                .AddNetCoreValidations()
                .AddScoped<ISiteContentRepository, SiteContentRepository>();

        public static IServiceCollection AddTestClients(this IServiceCollection services) => throw new NotImplementedException();

    }
}
