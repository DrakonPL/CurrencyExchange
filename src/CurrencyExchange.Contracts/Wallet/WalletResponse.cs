using CurrencyExchange.Contracts.Funds;

namespace CurrencyExchange.Contracts.Wallet
{
    public record WalletResponse(int Id,string Name, List<FundsResponse> Funds);
}
