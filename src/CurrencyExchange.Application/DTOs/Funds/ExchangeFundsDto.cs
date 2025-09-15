namespace CurrencyExchange.Application.DTOs.Funds
{
    public class ExchangeFundsDto
    {
        public int WalletId { get; set; }
        public int FromCurrencyId { get; set; }
        public int ToCurrencyId { get; set; }
        public decimal Amount { get; set; }
    }
}
