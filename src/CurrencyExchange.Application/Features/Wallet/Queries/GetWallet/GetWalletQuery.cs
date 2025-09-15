using CurrencyExchange.Application.DTOs.Wallet;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchange.Application.Features.Wallet.Queries.GetWallet
{
    public record GetWalletQuery(int Id) : IRequest<WalletDto>;
}
