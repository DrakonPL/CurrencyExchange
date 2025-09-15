using CurrencyExchange.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchange.Application.Contracts
{
    public interface IFundsRepository : IGenericRepository<Funds>
    {
    }
}
