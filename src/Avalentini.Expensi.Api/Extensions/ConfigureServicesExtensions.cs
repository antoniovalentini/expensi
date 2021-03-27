using System;
using System.Net;
using AutoMapper;
using Avalentini.Expensi.Core.Data.ApiContracts;
using Avalentini.Expensi.Core.Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                        await context.Response.WriteAsync(error).ConfigureAwait(false);
                    }
                });
            });
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = config["Authority"];
                options.Audience = config["Audience"];
            });

            return services;
        }

        public static IServiceCollection AddAuthorizationWithPolicy(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
                options.AddPolicy("userIdPolicy",
                    builder => builder.RequireClaim("http://schemas.xmlsoap.org/ws/2020/04/identity/claims/userid")));

            return services;
        }

        public static IServiceCollection AddAutoMapperWithMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(ConfigAction, typeof(Startup));
            return services;
        }

        private static void ConfigAction(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ExpenseMongoEntity, Expense>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(entity => entity.ExpenseId));
            cfg.CreateMap<Expense, ExpenseMongoEntity>()
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ExpenseId, opt => opt.MapFrom(src => src.Id));
        }
    }

    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
