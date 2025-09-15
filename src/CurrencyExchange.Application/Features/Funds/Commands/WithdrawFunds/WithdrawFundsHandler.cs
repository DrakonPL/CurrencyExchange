using AutoMapper;
using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Application.DTOs.Funds;
using CurrencyExchange.Application.DTOs.Funds.Validators;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var validator = new WithdrawFundsDtoValidator(walletRepository);
            var validationResult = await validator.ValidateAsync(request.WithdrawFundsDto);

            throw new NotImplementedException();
        }
    }
}
