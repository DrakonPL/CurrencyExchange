using CurrencyExchange.Application.Behaviors;
using CurrencyExchange.Application.Interfaces;
using CurrencyExchange.Application.Services;
using CurrencyExchange.Application.Worker;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CurrencyExchange.Application
{
    public static class AppServiceRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
                    .AddOpenBehavior(typeof(ValidationBehavior<,>))

            );
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            var nbpBaseUrl = configuration["ExternalApis:Nbp:BaseUrl"]
                 ?? throw new InvalidOperationException("NBP base URL not configured.");

            services.AddHttpClient<NbpClient>(c => c.BaseAddress = new Uri(nbpBaseUrl));
            services.AddHostedService<RatesWorker>();

            services.AddScoped<ICurrencyConverter, CurrencyConverter>();

            return services;
        }
    }
}
