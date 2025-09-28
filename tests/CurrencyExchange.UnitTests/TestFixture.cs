using AutoMapper;
using CurrencyExchange.Application.Contracts;
using CurrencyExchange.Application.Interfaces;
using CurrencyExchange.Application.Profiles;
using CurrencyExchange.Application.Services;
using CurrencyExchange.Domain.Entities;
using CurrencyExchange.Infrastructure;
using CurrencyExchange.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace CurrencyExchange.UnitTests
{
    public class TestFixture
    {
        private static readonly ILoggerFactory _loggerFactory =
        LoggerFactory.Create(builder =>
            builder
                .AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Information));


        public CurrencyExchangeDbContext Context { get; }

        public IUnitOfWork UnitOfWork { get; }
        public IWalletRepository WalletRepository { get; }
        public ICurrencyRepository CurrencyRepository { get; }
        public IFundsRepository FundsRepository { get; }
        public ICurrencyConverter CurrencyConverter { get; }
        public IMapper Mapper { get; }
        public IMemoryCache MemoryCache { get; }

        public TestFixture()
        {
            var options = new DbContextOptionsBuilder<CurrencyExchangeDbContext>()
                .UseInMemoryDatabase($"test_{Guid.NewGuid()}")
                .EnableSensitiveDataLogging() // helpful for diagnosing tracking issues
                .Options;

            Context = new CurrencyExchangeDbContext(options);
            Context.Database.EnsureCreated();

            UnitOfWork = new UnitOfWork(Context,
                new CurrencyRepository(Context),
                new WalletRepository(Context),
                new FundsRepository(Context));

            CurrencyRepository = UnitOfWork.CurrencyRepository;
            WalletRepository = UnitOfWork.WalletRepository;
            FundsRepository = UnitOfWork.FundsRepository;

            var mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            }, _loggerFactory);


            Mapper = mapperCfg.CreateMapper();

            MemoryCache = new MemoryCache(new MemoryCacheOptions());

            CurrencyConverter = new CurrencyConverter(CurrencyRepository);

            SeedBaseCurrencies();
        }

        private void SeedBaseCurrencies()
        {
            // PLN as base (rate 1), plus USD/EUR sample (NBP logic assumes mid rate to PLN)
            AddCurrency("PLN", "Polish Zloty", 1m);
            AddCurrency("USD", "US Dollar", 4.00m);
            AddCurrency("EUR", "Euro", 4.40m);
            Context.SaveChanges();
        }

        public Currency AddCurrency(string code, string name, decimal rate)
        {
            var c = new Currency (code, name, rate);
            Context.Currencies.Add(c);
            return c;
        }

        public Wallet AddWallet(string name)
        {
            var w = new Wallet { Name = name };
            Context.Wallets.Add(w);
            Context.SaveChanges();
            return w;
        }

        public void AddFunds(Wallet wallet, string currencyCode, decimal amount)
        {
            var currency = Context.Currencies.First(c => c.Code == currencyCode);
            wallet.Funds.Add(new Funds (wallet, currency, amount ));
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
