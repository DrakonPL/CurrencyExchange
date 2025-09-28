namespace CurrencyExchange.Contracts.Wallet
{
    public record TransactionResponse(
        int Id,
        int WalletId,
        string CurrencyCode,
        string Type,
        string Direction,
        decimal Amount,
        decimal RateAtTransaction,
        DateTime CreatedAtUtc,
        Guid? CorrelationId
    );
}
