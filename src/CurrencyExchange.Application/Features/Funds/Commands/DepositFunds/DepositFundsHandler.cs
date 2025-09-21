using AutoMapper;
using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Application.DTOs.Funds;
using MediatR;

namespace CurrencyExchange.Application.Features.Funds.Commands.DepositFunds
{
    public class DepositFundsHandler(
         IWalletRepository walletRepository,
         ICurrencyRepository currencyRepository,
         IMapper mapper
        ) : IRequestHandler<DepositFundsCommand, FundsDto>
    {
        public async Task<FundsDto> Handle(DepositFundsCommand request, CancellationToken cancellationToken)
        {
            var wallet = await walletRepository.Get(request.Id);
            var currency = await currencyRepository.GetByCode(request.DepositFundsDto.CurrencyCode);

            var funds = wallet.DepositFunds(currency, request.DepositFundsDto.Amount);

            await walletRepository.Update(wallet);
            return mapper.Map<FundsDto>(funds);
        }
    }
}
