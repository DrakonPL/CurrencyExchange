using CurrencyExchange.Domain.Exceptions;

namespace CurrencyExchange.Domain.Entities
{
    public class Wallet
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public virtual ICollection<Funds> Funds { get; private set; } = new List<Funds>();

        public Funds DepositFunds(Currency currency, decimal amount)
        {
            if (amount <= 0)
                throw new DomainValidationException("Deposit amount must be positive.");


            var funds = Funds.FirstOrDefault(f => f.CurrencyId == currency.Id);

            if (funds is null)
            {
                funds = new Funds(this,currency, amount);
                Funds.Add(funds);
            }
            else
            {
                funds.Increase(amount);
            }
            return funds;
        }

        public Funds WithdrawFunds(Currency currency, decimal amount)
        {
            if (amount <= 0)
                throw new DomainValidationException("Withdrawal amount must be positive.");

            var funds = Funds.FirstOrDefault(f => f.CurrencyId == currency.Id);
            var available = funds?.Amount ?? 0m;

            if (available < amount)
                throw new DomainValidationException(
                    $"Insufficient funds for currency {currency.Code}. Requested: {amount}, available: {available}.");


            funds!.Decrease(amount);
            return funds;
        }
    }
}

