using CurrencyExchange.Application.Contracts;
using MediatR;

namespace CurrencyExchange.Application.Features.Wallet.Commands.CreateWallet
{
    public class CreateWalletHandler(
            IWalletRepository walletRepository
        ) : IRequestHandler<CreateWalletCommand, int>
    {
        public async Task<int> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
        {
            var wallet = new Domain.Entities.Wallet
            {
                Name = request.CreateWalletDto.Name
            };

            wallet = await walletRepository.Add(wallet);
            return wallet.Id;
        }
    }
}
