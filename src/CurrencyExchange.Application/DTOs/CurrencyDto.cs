namespace CurrencyExchange.Application.DTOs
{
    public class CurrencyDto : BaseDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal ExcahangeRate { get; set; }
    }
}
