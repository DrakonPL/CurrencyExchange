namespace CurrencyExchange.Application.DTOs
{
    public class WalletDto : BaseDto
    {
        public required string Name { get; set; }
        public required List<FundsDto> Funds { get; set; }
    }
}
