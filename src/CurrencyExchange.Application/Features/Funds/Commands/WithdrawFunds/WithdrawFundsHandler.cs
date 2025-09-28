using AutoMapper;
using CurrencyExchange.Application.Common;
using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Application.DTOs;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace CurrencyExchange.Application.Features.Funds.Commands.WithdrawFunds
{
    public class WithdrawFundsHandler(
         IUnitOfWork unitOfWork,
         IMapper mapper,
         IMemoryCache memoryCache
        ) : IRequestHandler<WithdrawFundsCommand, FundsDto>
    {
        public async Task<FundsDto> Handle(WithdrawFundsCommand request, CancellationToken cancellationToken)
        {
            var wallet = await unitOfWork.WalletRepository.Get(request.WalletId);
            var currency = await unitOfWork.CurrencyRepository.GetByCode(request.CurrencyCode);

            var fundsLeft = wallet.WithdrawFunds(currency, request.Amount);
            unitOfWork.WalletRepository.Update(wallet);

            await unitOfWork.SaveAsync(cancellationToken);

            memoryCache.Remove(CacheKeys.WalletsAll);
            memoryCache.Remove(CacheKeys.WalletById(request.WalletId));

            return mapper.Map<FundsDto>(fundsLeft);
        }
    }
}
