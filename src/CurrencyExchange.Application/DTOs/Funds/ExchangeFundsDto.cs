namespace CurrencyExchange.Application.DTOs.Funds
{
    public class ExchangeFundsDto
    {
        public required int WalletId { get; set; }
        public required string FromCurrencyCode { get; set; }
        public required string ToCurrencyCode { get; set; }
        public required decimal Amount { get; set; }
    }
}
