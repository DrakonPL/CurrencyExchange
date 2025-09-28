using CurrencyExchange.Domain.Exceptions;

namespace CurrencyExchange.Domain.Entities
{
    public class Currency
    {
        public int Id { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }
        public decimal Rate { get; private set; }

        private Currency()
        {
            
        }

        public Currency(string code, string name, decimal rate)
        {
            Code = code;
            Name = name;
            Rate = rate;
        }

        public void UpdateRate(decimal newRate)
        {
            if (newRate <= 0)
                throw new DomainValidationException("Exchange rate must be positive.");

            Rate = newRate;
        }
    }
}
