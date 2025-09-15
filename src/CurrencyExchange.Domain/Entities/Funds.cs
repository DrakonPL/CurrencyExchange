namespace CurrencyExchange.Domain.Entities
{
    public class Funds
    {
        public int Id { get; set; }
        public int CurrencyId { get; set; }
        public Currency? Currency { get; set; }
        public decimal Amount { get; set; }
    }
}
