namespace CurrencyExchange.Application.DTOs.Funds
{
    public class FundsDto : BaseDto
    {
        public int WalletId { get; set; }
        public int CurrencyId { get; set; }
        public decimal Amount { get; set; }
    }
}
