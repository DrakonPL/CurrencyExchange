using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyExchange.Infrastructure
{
    public static class InfrastructureRegistraction
    {
        public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services)
        {
            services.AddDbContext<CurrencyExchangeDbContext>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IFundsRepository, FundsRepository>();

            return services;
        }
    }
}
