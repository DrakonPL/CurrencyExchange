namespace CurrencyExchange.Contracts.Funds
{
    public record ExchangeFundsRequest(
        string FromCurrencyCode,
        string ToCurrencyCode,
        decimal Amount);
}
