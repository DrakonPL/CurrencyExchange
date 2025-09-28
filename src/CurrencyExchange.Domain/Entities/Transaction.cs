using CurrencyExchange.Domain.Enums;
using CurrencyExchange.Domain.Exceptions;

namespace CurrencyExchange.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; private set; }

        public int WalletId { get; private set; }
        public virtual Wallet? Wallet { get; private set; }

        public int CurrencyId { get; private set; }
        public virtual Currency? Currency { get; private set; }

        public TransactionType Type { get; private set; }
        public TransactionDirection Direction { get; private set; }

        // Amount in the leg currency, always positive.
        public decimal Amount { get; private set; }

        // Snapshot of mid rate at the time of transaction (currency → PLN).
        public decimal RateAtTransaction { get; private set; }

        public DateTime CreatedAtUtc { get; private set; }


        // Correlates legs for Exchanges (same Guid for both legs).
        public Guid? CorrelationId { get; private set; }

        private Transaction() { } // EF

        public Transaction(Wallet wallet, Currency currency, TransactionType type, TransactionDirection direction, decimal amount, decimal rateAtTransaction, Guid? correlationId = null)
        {
            if (amount <= 0) 
                throw new DomainValidationException("Transaction amount must be positive.");

            WalletId = wallet.Id;
            Wallet = wallet;
            CurrencyId = currency.Id;
            Currency = currency;

            Type = type;
            Direction = direction;
            Amount = amount;
            RateAtTransaction = rateAtTransaction;
            CreatedAtUtc = DateTime.UtcNow;
            CorrelationId = correlationId;
        }
    }
}
