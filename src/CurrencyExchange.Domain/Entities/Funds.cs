namespace CurrencyExchange.Domain.Entities
{
    public class Funds
    {
        public int Id { get; private set; }
        public int WalletId { get; private set; }
        public virtual Wallet? Wallet { get; private set; }
        public int CurrencyId { get; private set; }
        public virtual Currency? Currency { get; private set; }
        public decimal Amount { get; private set; }   // was public set

        private Funds() { } // EF

        public Funds(Wallet wallet,Currency currency, decimal amount)
        {
            WalletId = wallet.Id;
            Wallet = wallet;
            CurrencyId = currency.Id;
            Currency = currency;
            Amount = amount;
        }

        internal void Increase(decimal value) => Amount += value;
        internal void Decrease(decimal value) => Amount -= value;
    }
}
