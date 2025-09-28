using CurrencyExchange.Api.Middlewares;
using CurrencyExchange.Application;
using CurrencyExchange.Infrastructure;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text.Json;

namespace CurrencyExchange.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Enable HTTP logging services
            builder.Services.AddHttpLogging(options =>
            {
                options.LoggingFields = HttpLoggingFields.Request | HttpLoggingFields.Response;
            });

            builder.Host.UseSerilog((context, loggerConfig) =>
                loggerConfig.ReadFrom.Configuration(context.Configuration)
            );

            // Add services to the container.
            builder.Services.ConfigureApplicationServices(builder.Configuration);
            builder.Services.ConfigureInfrastructureServices();

            builder.Services.AddControllers()
            .AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                o.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            });

            builder.Services.AddMemoryCache();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CurrencyExchange API",
                    Version = "v1",
                    Description = "API for wallet currency exchange operations."
                });
            });

            var app = builder.Build();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            // Always log requests via Serilog (structured)
            app.UseSerilogRequestLogging();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CurrencyExchange API v1");
                    c.DisplayRequestDuration();
                });

                app.UseHttpLogging();
            }

            app.UseHttpsRedirection();
            app.MapControllers();
            app.Run();
        }
    }
}
