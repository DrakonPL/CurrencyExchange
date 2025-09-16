namespace CurrencyExchange.Application.DTOs
{
    public class CurrencyDto
    {
        public required string Code { get; set; }
        public required string Name { get; set; }
        public decimal Rate { get; set; }
    }
}
