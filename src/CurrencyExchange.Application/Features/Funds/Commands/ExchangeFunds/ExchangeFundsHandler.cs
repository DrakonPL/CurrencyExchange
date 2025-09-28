using AutoMapper;
using CurrencyExchange.Application.Common;
using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Application.DTOs;
using CurrencyExchange.Application.Interfaces;
using CurrencyExchange.Domain.Entities;
using CurrencyExchange.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace CurrencyExchange.Application.Features.Funds.Commands.ExchangeFunds
{
    public class ExchangeFundsHandler(
        IUnitOfWork unitOfWork,
        ICurrencyConverter currencyConverter,
        IMapper mapper,
        IMemoryCache memoryCache
        ) : IRequestHandler<ExchangeFundsCommand, FundsDto>
    {
        public async Task<FundsDto> Handle(ExchangeFundsCommand request, CancellationToken cancellationToken)
        {
            var wallet = await unitOfWork.WalletRepository.Get(request.WalletId);
            var fromCurrency = await unitOfWork.CurrencyRepository.GetByCode(request.FromCurrencyCode);
            var toCurrency = await unitOfWork.CurrencyRepository.GetByCode(request.ToCurrencyCode);

            wallet.WithdrawFunds(fromCurrency, request.Amount);

            var exchangedAmount = await currencyConverter.Convert(
                fromCurrency.Code,
                toCurrency.Code,
                request.Amount,
                cancellationToken
            );

            var funds = wallet.DepositFunds(toCurrency, exchangedAmount);
            unitOfWork.WalletRepository.Update(wallet);

            // log both legs with same correlation id
            var correlationId = Guid.NewGuid();

            await unitOfWork.TransactionRepository.Add(new Transaction(
                wallet,
                fromCurrency,
                TransactionType.Exchange,
                TransactionDirection.Out,
                request.Amount,
                fromCurrency.Rate,
                correlationId
            ));

            await unitOfWork.TransactionRepository.Add(new Transaction(
                wallet,
                toCurrency,
                TransactionType.Exchange,
                TransactionDirection.In,
                Math.Round(exchangedAmount, 4), // keep precision similar to tests
                toCurrency.Rate,
                correlationId
            ));

            await unitOfWork.SaveAsync(cancellationToken);

            memoryCache.Remove(CacheKeys.WalletsAll);
            memoryCache.Remove(CacheKeys.WalletById(request.WalletId));

            return mapper.Map<FundsDto>(funds);
        }
    }
}
