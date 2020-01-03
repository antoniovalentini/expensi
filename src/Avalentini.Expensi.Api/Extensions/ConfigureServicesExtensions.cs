using System;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Avalentini.Expensi.Api.Extensions
{
    public static class ConfigureServicesExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
 
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if(contextFeature != null)
                    { 
                        Console.WriteLine($"Something went wrong: {contextFeature.Error.Message}", contextFeature.Error);
 
                        var error = JsonConvert.SerializeObject(new ErrorDetails
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error."
                        });
                        await context.Response.WriteAsync(error);
                    }
                });
            });
        }

        public static void AddMongoDbCollection<T>(this IServiceCollection services, string mongoConnectionString, string mongoDbName, string mongoCollectionName)
        {
            var client = new MongoClient(mongoConnectionString);
            var database = client.GetDatabase(mongoDbName);
            var collection = database.GetCollection<T>(mongoCollectionName);
            services.AddSingleton(collection);
        }
    }

    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
