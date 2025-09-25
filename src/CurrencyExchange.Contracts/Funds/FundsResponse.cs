namespace CurrencyExchange.Contracts.Funds
{
    public record FundsResponse(
        string CurrencyCode,
        decimal Amount);
}
