using CurrencyExchange.Application.DTOs.Funds;

namespace CurrencyExchange.Application.DTOs.Wallet
{
    public class WalletDto : BaseDto
    {
        public required string Name { get; set; }
        public List<FundsDto>? Funds { get; set; }
    }
}
