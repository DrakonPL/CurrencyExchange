using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchange.Domain.Entities
{
    public class Funds
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }
    }
}
