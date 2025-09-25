namespace CurrencyExchange.Contracts.Funds
{
    public record DepositFundsRequest(string CurrencyCode, decimal Amount);
}
