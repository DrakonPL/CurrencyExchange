using CurrencyExchange.Application.DTOs;
using MediatR;

namespace CurrencyExchange.Application.Features.Wallet.Queries.GetWalletTransactions
{
    public record GetWalletTransactionsQuery(int WalletId, int? Take = null, int? Skip = null) : IRequest<List<TransactionDto>>;
}
