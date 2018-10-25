using AutoSite.Services;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AutoSite.Business.Extensions
{
    public static class ClientExtensions
    {

        static IConfigurationSection QnA(this IConfiguration configuration)
                            => configuration.GetSection("QnA");

        static IConfigurationSection TypeMapper(this IConfigurationSection configuration)
                        => configuration.GetSection("typeMapper");

        static IConfigurationSection Cognitive(this IConfiguration configuration)
                    => configuration.GetSection("cognitive");

        static IConfigurationSection EntityUnderstanding(this IConfigurationSection configuration)
                => configuration.GetSection("entityUnderstanding");

        static IConfigurationSection Vision(this IConfiguration configuration)
                            => configuration.GetSection("vision");

        static IConfigurationSection CustomVision(this IConfiguration configuration)
                    => configuration.GetSection("customVision");


        internal static IServiceCollection AddTypeMapperClient(
                                                        this IServiceCollection services,
                                                        IConfiguration config
                                                    )
        {
            var configuration = config.QnA();
            var mapperConfig = configuration.TypeMapper();
            services.AddHttpClient<ITypeSuggester, TypeSuggester>(
                client => {
                    client.BaseAddress = new Uri(configuration["base"]);
                    client.DefaultRequestHeaders.Add("Authorization", mapperConfig["Authorization"]);
                }
            );
            return services.AddSingleton(new TypeSuggester.Options {
                Path = mapperConfig["path"]
            });
        }

        internal static IServiceCollection AddEntityUnderstandingClient (
                                                                this IServiceCollection services,
                                                                IConfiguration configuration
                                                            )
        {
            var cogConfig = configuration.Cognitive();
            var understandConfig = cogConfig.EntityUnderstanding();

            services.AddHttpClient<IEntityUnderstanding, EntityUnderstanding>(
                    client => {
                        client.BaseAddress = new Uri(cogConfig["luisBase"]);
                        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", 
                                                understandConfig["subscription-key"]);
                    }
                );
            return services.AddSingleton(new EntityUnderstanding.Options {
                Path = understandConfig["path"]
            });
        }

        internal static IServiceCollection AddVisionClient(
                                                this IServiceCollection services,
                                                IConfiguration configuration
            )
        {
            var config = configuration.Vision();
            ComputerVisionClient computerVision = new ComputerVisionClient(
                            new ApiKeyServiceClientCredentials(config["key"]),
                            new DelegatingHandler[] { })
            {
                Endpoint = config["endPoint"]
            };

            services.AddHttpClient<IImageReader, ImageReader>(client => {
                client.BaseAddress = new Uri(config["endPoint"]);
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", config["key"]);
            });
            return services.AddSingleton(computerVision);
        }

        internal static IServiceCollection AddCustomVisionClient (
                                                this IServiceCollection services,
                                                IConfiguration configuration
                                         )
        {
            var config = configuration.CustomVision();
            services.AddHttpClient<ICustomVisionReader, CustomVisionReader>(
                    client => {
                        client.BaseAddress = new Uri(config["base"]);
                        client.DefaultRequestHeaders.Add("Prediction-Key", config["key"]);
                    }
                );
            return services.AddSingleton(new CustomVisionReader.Options {
                Path = config["imagePath"]
            });
        }
        
    }
}
