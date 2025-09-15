using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CurrencyExchange.Infrastructure.Worker
{
    public class RatesWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _interval = TimeSpan.FromHours(4);

        public RatesWorker(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

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
            using var scope = _scopeFactory.CreateScope();
            var nbpClient = scope.ServiceProvider.GetRequiredService<NbpClient>();
            var currencyRepository = scope.ServiceProvider.GetRequiredService<ICurrencyRepository>();

            try
            {
                var tables = await nbpClient.GetTableBAsync(ct);
                foreach (var t in tables)
                {
                    foreach (var r in t.rates)
                    {
                        var currency = await currencyRepository.GetByCode(r.code);
                        if (currency == null)
                        {
                            currency = new Currency
                            {
                                Code = r.code,
                                Name = r.currency,
                                Rate = r.mid,
                            };
                            await currencyRepository.Add(currency);
                        }
                        else
                        {
                            currency.Rate = r.mid;
                            await currencyRepository.Update(currency);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}
