namespace CurrencyExchange.Application.DTOs.Funds
{
    public class DepositFundsDto
    {
        public int WalletId { get; set; }
        public required string CurrencyCode { get; set; }
        public decimal Amount { get; set; }
    }
}
