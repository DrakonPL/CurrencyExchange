namespace CurrencyExchange.Application.Common
{
    public static class CacheKeys
    {
        public const string CurrenciesAll = "currencies:all";
        public const string WalletsAll = "wallets:all";
        public static string WalletById(int id) => $"wallets:{id}";

        public static TimeSpan CacheDuration { get; } = TimeSpan.FromMinutes(5);
    }
}
