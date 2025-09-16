using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Application.DTOs.Wallet.Validators;
using FluentValidation;
using MediatR;

namespace CurrencyExchange.Application.Features.Wallet.Commands.CreateWallet
{
    public class CreateWalletHandler(
            IWalletRepository walletRepository
        ) : IRequestHandler<CreateWalletCommand, int>
    {
        public async Task<int> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
        {
            //var validator = new CreateWalletDtoValidator();
            //var validationResult = await validator.ValidateAsync(request.CreateWalletDto, cancellationToken);

            //if (!validationResult.IsValid)
            //{
            //    throw new ValidationException(validationResult.Errors);
            //}

            var wallet = new Domain.Entities.Wallet
            {
                Name = request.CreateWalletDto.Name
            };

            wallet = await walletRepository.Add(wallet);
            return wallet.Id;
        }
    }
}
