using CurrencyExchange.Application.DTOs.Funds;

namespace CurrencyExchange.Application.DTOs.Wallet
{
    public class WalletDto : BaseDto
    {
        public string Name { get; set; }
        public List<FundsDto> Funds { get; set; }
    }
}
