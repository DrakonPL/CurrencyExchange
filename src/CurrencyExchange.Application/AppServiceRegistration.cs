using CurrencyExchange.Application.Interfaces;
using CurrencyExchange.Application.Services;
using CurrencyExchange.Application.Worker;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CurrencyExchange.Application
{
    public static class AppServiceRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            //services.AddHttpClient<NbpClient>(c => c.BaseAddress = new Uri("https://api.nbp.pl"));
            //services.AddHostedService<RatesWorker>();

            services.AddScoped<ICurrencyConverter, CurrencyConverter>();

            return services;
        }
    }
}
