using AutoMapper;
using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Application.DTOs.Funds;
using CurrencyExchange.Application.DTOs.Funds.Validators;
using FluentValidation;
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
            var validator  = new DepositFundsDtoValidator(walletRepository, currencyRepository);
            var validationResult = await validator.ValidateAsync(request.DepositFundsDto);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var wallet = await walletRepository.Get(request.DepositFundsDto.WalletId);
            var currency = await currencyRepository.GetByCode(request.DepositFundsDto.CurrencyCode);

            var funds = wallet.Funds.Where(f => f.CurrencyId == currency.Id).FirstOrDefault();

            if (funds != null)
            {
                funds.Amount += request.DepositFundsDto.Amount;
            }
            else
            {
                funds = new Domain.Entities.Funds
                {
                    CurrencyId = currency.Id,
                    Amount = request.DepositFundsDto.Amount
                };

                wallet.Funds.Add(funds);
            }

            await walletRepository.Update(wallet);
            return mapper.Map<FundsDto>(funds);
        }
    }
}
