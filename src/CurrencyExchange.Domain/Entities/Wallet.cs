namespace CurrencyExchange.Domain.Entities
{
    public class Wallet
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<Funds> Funds { get; set; } = new List<Funds>();
    }
}
