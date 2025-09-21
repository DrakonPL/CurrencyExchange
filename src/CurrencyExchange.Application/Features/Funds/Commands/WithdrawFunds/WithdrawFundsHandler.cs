using AutoMapper;
using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Application.DTOs.Funds;
using MediatR;

namespace CurrencyExchange.Application.Features.Funds.Commands.WithdrawFunds
{
    public class WithdrawFundsHandler(
         IWalletRepository walletRepository,
         ICurrencyRepository currencyRepository,
         IMapper mapper
        ) : IRequestHandler<WithdrawFundsCommand, FundsDto>
    {
        public async Task<FundsDto> Handle(WithdrawFundsCommand request, CancellationToken cancellationToken)
        {
            var wallet = await walletRepository.Get(request.Id);
            var currency = await currencyRepository.GetByCode(request.WithdrawFundsDto.CurrencyCode);

            var fundsLeft = wallet.WithdrawFunds(currency, request.WithdrawFundsDto.Amount);
            await walletRepository.Update(wallet);

            return mapper.Map<FundsDto>(fundsLeft);
        }
    }
}
