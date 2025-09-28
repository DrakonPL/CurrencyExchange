using AutoMapper;
using CurrencyExchange.Application.Common;
using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Application.DTOs;
using CurrencyExchange.Domain.Entities;
using CurrencyExchange.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace CurrencyExchange.Application.Features.Funds.Commands.DepositFunds
{
    public class DepositFundsHandler(
         IUnitOfWork unitOfWork,
         IMapper mapper,
         IMemoryCache memoryCache,
         ILogger<DepositFundsHandler> logger
        ) : IRequestHandler<DepositFundsCommand, FundsDto>
    {
        public async Task<FundsDto> Handle(DepositFundsCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deposit start WalletId={WalletId} Currency={Currency} Amount={Amount}", request.WalletId, request.CurrencyCode, request.Amount);

            var wallet = await unitOfWork.WalletRepository.Get(request.WalletId);
            var currency = await unitOfWork.CurrencyRepository.GetByCode(request.CurrencyCode);

            var funds = wallet.DepositFunds(currency, request.Amount);
            unitOfWork.WalletRepository.Update(wallet);

            // log transaction
            await unitOfWork.TransactionRepository.Add(new Transaction(
                wallet,
                currency,
                TransactionType.Deposit,
                TransactionDirection.In,
                request.Amount,
                currency.Rate
            ));

            await unitOfWork.SaveAsync(cancellationToken);

            memoryCache.Remove(CacheKeys.WalletsAll);
            memoryCache.Remove(CacheKeys.WalletById(request.WalletId));

            logger.LogInformation("Deposit done WalletId={WalletId} Currency={Currency} NewAmount={NewAmount}", request.WalletId, funds.Currency.Code, funds.Amount);

            return mapper.Map<FundsDto>(funds);
        }
    }
}
