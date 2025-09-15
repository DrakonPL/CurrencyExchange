namespace CurrencyExchange.Application.DTOs.Funds
{
    public class FundsDto : BaseDto
    {
        public string CurrencyCode { get; set; }
        public decimal Amount { get; set; }
    }
}
