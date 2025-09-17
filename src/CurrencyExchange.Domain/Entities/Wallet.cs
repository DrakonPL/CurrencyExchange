using CurrencyExchange.Common.Exceptions;

namespace CurrencyExchange.Domain.Entities
{
    public class Wallet
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<Funds> Funds { get; set; } = new List<Funds>();

        public Funds DepositFunds(Currency currency, decimal amount)
        {
            var funds = Funds.FirstOrDefault(f => f.CurrencyId == currency.Id);

            if (funds != null)
            {
                funds.Amount += amount;
            }
            else
            {
                funds = new Funds
                {
                    CurrencyId = currency.Id,
                    Currency = currency,
                    Amount = amount
                };

                Funds.Add(funds);
            }

            return funds;
        }

        public Funds WithdrawFunds(Currency currency, decimal amount)
        {
            var funds = Funds.FirstOrDefault(f => f.CurrencyId == currency.Id);

            if (funds != null)
            {
                if (funds.Amount < amount)
                {
                    throw new BadRequestException("Insufficient funds in  wallet");
                }

                funds.Amount -= amount;
            }
            else
            {
                throw new BadRequestException("No funds available in the specified currency.");
            }

            return funds;
        }
    }
}
