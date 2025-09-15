namespace CurrencyExchange.Application.DTOs.Funds
{
    public class WithdrawFundsDto
    {
        public required int WalletId { get; set; }
        public required string CurrencyCode { get; set; }
        public required decimal Amount { get; set; }
    }
}
