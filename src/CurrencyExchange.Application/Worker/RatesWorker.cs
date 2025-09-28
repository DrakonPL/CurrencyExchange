using CurrencyExchange.Application.Common;
using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CurrencyExchange.Application.Worker
{
    public class RatesWorker(IServiceScopeFactory scopeFactory, IMemoryCache memoryCache, ILogger<RatesWorker> logger) : BackgroundService
    {
        private readonly TimeSpan _interval = TimeSpan.FromHours(4);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // First run on startup
            await RunWithResilience(FetchOnce, stoppingToken);

            using var timer = new PeriodicTimer(_interval);
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await RunWithResilience(FetchOnce, stoppingToken);
            }
        }

        private async Task RunWithResilience(Func<CancellationToken, Task> action, CancellationToken ct)
        {
            const int maxAttempts = 3;
            var delay = TimeSpan.FromSeconds(2);

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    await action(ct);
                    return;
                }
                catch (OperationCanceledException) when (ct.IsCancellationRequested)
                {
                    logger.LogInformation("RatesWorker cancelled.");
                    return;
                }
                catch (HttpRequestException ex)
                {
                    logger.LogWarning(ex, "Network error while fetching rates (attempt {Attempt}/{MaxAttempts}).", attempt, maxAttempts);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Unexpected error while fetching rates (attempt {Attempt}/{MaxAttempts}).", attempt, maxAttempts);
                }

                if (attempt < maxAttempts)
                {
                    try { await Task.Delay(delay, ct); } catch (OperationCanceledException) { return; }
                    delay = TimeSpan.FromSeconds(Math.Min(delay.TotalSeconds * 2, 60)); // exponential backoff with cap
                }
                else
                {
                    logger.LogError("All retries exhausted. Will try again on the next interval.");
                }
            }
        }

        private async Task FetchOnce(CancellationToken ct)
        {
            using var scope = scopeFactory.CreateScope();
            var nbpClient = scope.ServiceProvider.GetRequiredService<NbpClient>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var currencyRepository = unitOfWork.CurrencyRepository;

            var tables = await nbpClient.GetTableBAsync(ct);

            int added = 0, updated = 0, failed = 0;

            foreach (var t in tables)
            {
                foreach (var r in t.Rates)
                {
                    try
                    {
                        var currency = await currencyRepository.GetByCode(r.Code);
                        if (currency == null)
                        {
                            await currencyRepository.Add(new Currency(r.Code, r.Currency, r.Mid));
                            added++;
                        }
                        else
                        {
                            currency.UpdateRate(r.Mid);
                            currencyRepository.Update(currency);
                            updated++;
                        }
                    }
                    catch (OperationCanceledException) when (ct.IsCancellationRequested)
                    {
                        logger.LogInformation("Rates update cancelled.");
                        throw;
                    }
                    catch (Exception ex)
                    {
                        failed++;
                        logger.LogWarning(ex, "Failed to upsert currency {Code}. Skipping.", r.Code);
                    }
                }
            }

            await unitOfWork.SaveAsync(ct);

            memoryCache.Remove(CacheKeys.CurrenciesAll);
            logger.LogInformation("Rates fetch complete. Added: {Added}, Updated: {Updated}, Failed: {Failed}.", added, updated, failed);
        }
    }
}
