using AutoMapper;
using Avalentini.Expensi.Api.Contracts.Models;
using Avalentini.Expensi.Core.Data.Entities;
using Avalentini.Expensi.Api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Avalentini.Expensi.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connString = Configuration["MongoDbConnection"];
            services.AddMongoDbCollection<ExpensesPerUser>(connString,
                "expensi", "expenses");

            services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<ExpenseMongoEntity, Expense>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom( entity => entity.ExpenseId));
                cfg.CreateMap<Expense, ExpenseMongoEntity>()
                    .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                    .ForMember(dest => dest.ExpenseId, opt => opt.MapFrom(src => src.Id));
            }, typeof(Startup));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = Configuration["Authority"];
                options.Audience = Configuration["Audience"];
            });

            services.AddAuthorization(); 

            services.AddCors();
            services.AddMvcCore().AddApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Expensi API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.ConfigureExceptionHandler();

            //TODO: remember to fix this in release
            app.UseCors(builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Expensi API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
