namespace CurrencyExchange.Application.Interfaces
{
    public interface ICurrencyConverter
    {
        Task<decimal> Convert(string fromCurrencyCode, string toCurrencyCode, decimal amount, CancellationToken ct = default);
    }
}
