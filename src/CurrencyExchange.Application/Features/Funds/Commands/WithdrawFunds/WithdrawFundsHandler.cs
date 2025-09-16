using AutoMapper;
using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Application.DTOs.Funds;
using FluentValidation;
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
            var wallet = await walletRepository.Get(request.WithdrawFundsDto.WalletId);
            var currency = await currencyRepository.GetByCode(request.WithdrawFundsDto.CurrencyCode);

            var funds = wallet.Funds.FirstOrDefault(f => f.CurrencyId == currency.Id);

            if (funds != null)
            {
                if (funds.Amount < request.WithdrawFundsDto.Amount)
                {
                    throw new ValidationException("Insufficient funds.");
                }
                funds.Amount -= request.WithdrawFundsDto.Amount;
            }
            else
            {
                throw new ValidationException("No funds available in the specified currency.");
            }

            await walletRepository.Update(wallet);
            return mapper.Map<FundsDto>(funds);
        }
    }
}
