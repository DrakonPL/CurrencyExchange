using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CurrencyExchange.Application
{
    public class AppServiceRegistration
    {
        public static IServiceCollection ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }
    }
}
