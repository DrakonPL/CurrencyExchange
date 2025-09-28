using AutoMapper;
using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Application.DTOs;
using MediatR;

namespace CurrencyExchange.Application.Features.Wallet.Queries.GetWalletTransactions
{
    public class GetWalletTransactionsHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) : IRequestHandler<GetWalletTransactionsQuery, List<TransactionDto>>
    {
        public async Task<List<TransactionDto>> Handle(GetWalletTransactionsQuery request, CancellationToken cancellationToken)
        {
            var items = await unitOfWork.TransactionRepository.GetByWalletId(request.WalletId, request.Take, request.Skip);
            return mapper.Map<List<TransactionDto>>(items);
        }
    }
}
