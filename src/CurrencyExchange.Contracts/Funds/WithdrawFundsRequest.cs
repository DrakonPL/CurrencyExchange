namespace CurrencyExchange.Contracts.Funds
{
    public record WithdrawFundsRequest(string CurrencyCode, decimal Amount);
}
