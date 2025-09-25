using CurrencyExchange.Application.Common;
using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CurrencyExchange.Application.Worker
{
    public class RatesWorker(IServiceScopeFactory scopeFactory, IMemoryCache memoryCache) : BackgroundService
    {
        private readonly TimeSpan _interval = TimeSpan.FromHours(4);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // First run on startup
            await FetchOnce(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try { await Task.Delay(_interval, stoppingToken); } catch { }
                await FetchOnce(stoppingToken);
            }
        }

        private async Task FetchOnce(CancellationToken ct)
        {
            using var scope = scopeFactory.CreateScope();
            var nbpClient = scope.ServiceProvider.GetRequiredService<NbpClient>();
            var currencyRepository = scope.ServiceProvider.GetRequiredService<ICurrencyRepository>();

            try
            {
                var tables = await nbpClient.GetTableBAsync(ct);
                foreach (var t in tables)
                {
                    foreach (var r in t.Rates)
                    {
                        var currency = await currencyRepository.GetByCode(r.Code);
                        if (currency == null)
                        {
                            currency = new Currency
                            {
                                Code = r.Code,
                                Name = r.Currency,
                                Rate = r.Mid,
                            };
                            await currencyRepository.Add(currency);
                        }
                        else
                        {
                            currency.Rate = r.Mid;
                            await currencyRepository.Update(currency);
                        }
                    }
                }

                memoryCache.Remove(CacheKeys.CurrenciesAll);
            }
            catch (Exception ex)
            {

            }
        }

    }
}
