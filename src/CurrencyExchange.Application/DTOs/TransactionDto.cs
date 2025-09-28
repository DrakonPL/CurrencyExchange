namespace CurrencyExchange.Application.DTOs
{
    public class TransactionDto : BaseDto
    {
        public int WalletId { get; set; }
        public string CurrencyCode { get; set; } = default!;
        public string Type { get; set; } = default!;
        public string Direction { get; set; } = default!;
        public decimal Amount { get; set; }
        public decimal RateAtTransaction { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public Guid? CorrelationId { get; set; }
    }
}
