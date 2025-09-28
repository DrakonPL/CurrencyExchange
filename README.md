# CurrencyExchange

Minimal wallet-based currency exchange API (.NET 8, EF Core Sqlite, MediatR, AutoMapper, FluentValidation, BackgroundService).

## Architecture
Projects:
- Api: HTTP layer + Swagger + error middleware ([src/CurrencyExchange.Api](src/CurrencyExchange.Api))
- Application: CQRS (MediatR), DTOs, validators, background rates worker ([src/CurrencyExchange.Application](src/CurrencyExchange.Application))
- Infrastructure: EF Core repositories, Sqlite context, Unit of Work ([src/CurrencyExchange.Infrastructure](src/CurrencyExchange.Infrastructure))
- Domain: Entities (Wallet, Funds, Currency, Transaction) + domain exceptions ([src/CurrencyExchange.Domain](src/CurrencyExchange.Domain))
- Common: Shared exceptions ([src/CurrencyExchange.Common](src/CurrencyExchange.Common))
- Contracts: API contracts ([src/CurrencyExchange.Contracts](src/CurrencyExchange.Contracts))
- UnitTests: In-memory tests ([tests/CurrencyExchange.UnitTests](tests/CurrencyExchange.UnitTests))

Key components:
- Controllers: [`CurrencyExchange.Api.Controllers.WalletController`](src/CurrencyExchange.Api/Controllers/WalletController.cs), [`CurrencyExchange.Api.Controllers.CurrencyController`](src/CurrencyExchange.Api/Controllers/CurrencyController.cs)
- Handlers:
  - [`CurrencyExchange.Application.Features.Funds.Commands.DepositFunds.DepositFundsHandler`](src/CurrencyExchange.Application/Features/Funds/Commands/DepositFunds/DepositFundsHandler.cs)
  - [`CurrencyExchange.Application.Features.Funds.Commands.WithdrawFunds.WithdrawFundsHandler`](src/CurrencyExchange.Application/Features/Funds/Commands/WithdrawFunds/WithdrawFundsHandler.cs)
  - [`CurrencyExchange.Application.Features.Funds.Commands.ExchangeFunds.ExchangeFundsHandler`](src/CurrencyExchange.Application/Features/Funds/Commands/ExchangeFunds/ExchangeFundsHandler.cs)
  - [`CurrencyExchange.Application.Features.Wallet.Commands.CreateWallet.CreateWalletHandler`](src/CurrencyExchange.Application/Features/Wallet/Commands/CreateWallet/CreateWalletHandler.cs)
  - [`CurrencyExchange.Application.Features.Wallet.Queries.GetWallet.GetWalletHandler`](src/CurrencyExchange.Application/Features/Wallet/Queries/GetWallet/GetWalletHandler.cs)
  - [`CurrencyExchange.Application.Features.Wallet.Queries.GetAllWallets.GetAllWalletsHandler`](src/CurrencyExchange.Application/Features/Wallet/Queries/GetAllWallets/GetAllWalletsHandler.cs)
  - Wallet transactions query: [`CurrencyExchange.Application.Features.Wallet.Queries.GetWalletTransactions`](src/CurrencyExchange.Application/Features/Wallet/Queries/GetWalletTransactions)
- Validators:
  - [`CurrencyExchange.Application.Features.Funds.Commands.DepositFunds.DepositFundsValidator`](src/CurrencyExchange.Application/Features/Funds/Commands/DepositFunds/DepositFundsValidator.cs)
  - [`CurrencyExchange.Application.Features.Funds.Commands.WithdrawFunds.WithdrawFundsValidator`](src/CurrencyExchange.Application/Features/Funds/Commands/WithdrawFunds/WithdrawFundsValidator.cs)
  - [`CurrencyExchange.Application.Features.Funds.Commands.ExchangeFunds.ExchangeFundsValidator`](src/CurrencyExchange.Application/Features/Funds/Commands/ExchangeFunds/ExchangeFundsValidator.cs)
  - [`CurrencyExchange.Application.Features.Wallet.Commands.CreateWallet.CreateWalletValidator`](src/CurrencyExchange.Application/Features/Wallet/Commands/CreateWallet/CreateWalletValidator.cs)
  - [`CurrencyExchange.Application.Features.Wallet.Queries.GetWallet.GetWalletValidator`](src/CurrencyExchange.Application/Features/Wallet/Queries/GetWallet/GetWalletValidator.cs)
- Background rates: [`CurrencyExchange.Application.Worker.RatesWorker`](src/CurrencyExchange.Application/Worker/RatesWorker.cs) + [`CurrencyExchange.Application.Worker.NbpClient`](src/CurrencyExchange.Application/Worker/NbpClient.cs)
- Conversion service: [`CurrencyExchange.Application.Services.CurrencyConverter`](src/CurrencyExchange.Application/Services/CurrencyConverter.cs)
- DbContext: [`CurrencyExchange.Infrastructure.CurrencyExchangeDbContext`](src/CurrencyExchange.Infrastructure/CurrencyExchangeDbContext.cs)
- Error middleware: [`CurrencyExchange.Api.Middlewares.ErrorHandlerMiddleware`](src/CurrencyExchange.Api/Middlewares/ErrorHandlerMiddleware.cs)
- Caching keys: [`CurrencyExchange.Application.Common.CacheKeys`](src/CurrencyExchange.Application/Common/CacheKeys.cs)
- Unit of Work: [`CurrencyExchange.Application.Contracts.IUnitOfWork`](src/CurrencyExchange.Application/Contracts/IUnitOfWork.cs), [`CurrencyExchange.Infrastructure.UnitOfWork`](src/CurrencyExchange.Infrastructure/UnitOfWork.cs)

## Data Model
Entities:
- [`Currency`](src/CurrencyExchange.Domain/Entities/Currency.cs) (Code, Name, Rate; PLN-based mid rate)
- [`Wallet`](src/CurrencyExchange.Domain/Entities/Wallet.cs) (Id, Name, Funds; domain methods for deposit/withdraw with validation)
- [`Funds`](src/CurrencyExchange.Domain/Entities/Funds.cs) (WalletId, CurrencyId, Amount; controlled mutations)
- [`Transaction`](src/CurrencyExchange.Domain/Entities/Transaction.cs) (WalletId, CurrencyId, Amount, Type, Direction, RateAtTransaction, CreatedAtUtc, CorrelationId)

All currency conversion assumes PLN as implicit base:
$amount_{target} = \dfrac{amount \times rate_{from}}{rate_{to}}$

Transaction history notes:
- One row per leg. Exchange = two legs with shared CorrelationId.
- Logged in command handlers (deposit/withdraw/exchange) and exposed via a wallet transactions query.

## Background Rates
- Runs every 4h; first run triggers on startup: [`RatesWorker`](src/CurrencyExchange.Application/Worker/RatesWorker.cs)
- Fetches NBP Table B via [`NbpClient`](src/CurrencyExchange.Application/Worker/NbpClient.cs)
- Configured in [`AppServiceRegistration`](src/CurrencyExchange.Application/AppServiceRegistration.cs) and reads base URL from [`appsettings.json`](src/CurrencyExchange.Api/appsettings.json) at ExternalApis:Nbp:BaseUrl

## Persistence & Unit of Work
- Repositories do not call SaveChanges. Commit boundary is per command via [`IUnitOfWork.SaveAsync`](src/CurrencyExchange.Application/Contracts/IUnitOfWork.cs).
- Handlers mutate aggregates and then commit once, e.g.:
  - Create wallet: [`CreateWalletHandler`](src/CurrencyExchange.Application/Features/Wallet/Commands/CreateWallet/CreateWalletHandler.cs)
  - Deposit: [`DepositFundsHandler`](src/CurrencyExchange.Application/Features/Funds/Commands/DepositFunds/DepositFundsHandler.cs)
  - Withdraw: [`WithdrawFundsHandler`](src/CurrencyExchange.Application/Features/Funds/Commands/WithdrawFunds/WithdrawFundsHandler.cs)
  - Exchange: [`ExchangeFundsHandler`](src/CurrencyExchange.Application/Features/Funds/Commands/ExchangeFunds/ExchangeFundsHandler.cs)
- Read queries use AsNoTracking where appropriate, e.g. [`WalletRepository`](src/CurrencyExchange.Infrastructure/Repositories/WalletRepository.cs).

## Endpoints
Base route style: /{ControllerName}
- GET /Currency → list all currencies
- GET /Wallet → list all wallets with funds
- GET /Wallet/{id} → get wallet by id
- GET /Wallet/{id}/transactions → list wallet transaction history (with optional take/skip)
- POST /Wallet (body: { "name": "My Wallet" }) → creates wallet (returns new id)
- POST /Wallet/{id}/deposit (body: { "currencyCode": "USD", "amount": 100 })
- POST /Wallet/{id}/withdraw (body: { "currencyCode": "USD", "amount": 25 })
- POST /Wallet/{id}/exchange (body: { "fromCurrencyCode": "USD", "toCurrencyCode": "EUR", "amount": 50 })

See: [`WalletController`](src/CurrencyExchange.Api/Controllers/WalletController.cs)

Sample requests file: [src/CurrencyExchange.Api/CurrencyExchange.Api.http](src/CurrencyExchange.Api/CurrencyExchange.Api.http)

## Validation & Errors
- Request validation via FluentValidation; executed as MediatR pipeline: [`ValidationBehavior`](src/CurrencyExchange.Application/Behaviors/ValidationBehavior.cs)
- Errors mapped by middleware: [`ErrorHandlerMiddleware`](src/CurrencyExchange.Api/Middlewares/ErrorHandlerMiddleware.cs)
  - 400 (request): [`BadRequestException`](src/CurrencyExchange.Application/Exceptions/BadRequestException.cs)
  - 404: [`EntityNotFoundException`](src/CurrencyExchange.Application/Exceptions/EntityNotFoundException.cs)
  - 400 (domain rules): [`DomainValidationException`](src/CurrencyExchange.Domain/Exceptions/DomainValidationException.cs)
- Cache invalidation on write operations via [`CacheKeys`](src/CurrencyExchange.Application/Common/CacheKeys.cs)

## Build & Run
Restore & run API with Swagger UI:
```bash
dotnet restore
dotnet run --project src/CurrencyExchange.Api
```

## Database (EF Core Sqlite)
Add migration:
```bash
dotnet ef migrations add <Name> --project src/CurrencyExchange.Infrastructure --startup-project src/CurrencyExchange.Api --output-dir Migrations
```
Apply:
```bash
dotnet ef database update --project src/CurrencyExchange.Infrastructure --startup-project src/CurrencyExchange.Api
```
Database file is stored in local app data (see [`CurrencyExchangeDbContext`](src/CurrencyExchange.Infrastructure/CurrencyExchangeDbContext.cs)). Design-time factory: [`DesignTimeDbContextFactory`](src/CurrencyExchange.Infrastructure/DesignTimeDbContextFactory.cs).

## Tests
Run unit tests:
```bash
dotnet test
```
Tests use the EF InMemory provider with a fixture that seeds sample currencies: [`TestFixture`](tests/CurrencyExchange.UnitTests/TestFixture.cs). Coverage includes handlers, validators, and currency conversion:
- [`DepositFundsHandlerTests`](tests/CurrencyExchange.UnitTests/Features/Funds/DepositFundsHandlerTests.cs)
- [`WithdrawFundsHandlerTests`](tests/CurrencyExchange.UnitTests/Features/Funds/WithdrawFundsHandlerTests.cs)
- [`ExchangeFundsHandlerTests`](tests/CurrencyExchange.UnitTests/Features/Funds/ExchangeFundsHandlerTests.cs)
- [`DepositFundsValidatorTests`](tests/CurrencyExchange.UnitTests/Features/Funds/DepositFundsValidatorTests.cs)
- [`WithdrawFundsValidatorTests`](tests/CurrencyExchange.UnitTests/Features/Funds/WithdrawFundsValidatorTests.cs)
- [`ExchangeFundsValidatorTests`](tests/CurrencyExchange.UnitTests/Features/Funds/ExchangeFundsValidatorTests.cs)
- [`CurrencyConverterTests`](tests/CurrencyExchange.UnitTests/Services/CurrencyConverterTests.cs)
- [`CreateWalletHandlerTests`](tests/CurrencyExchange.UnitTests/Features/Wallet/CreateWalletHandlerTests.cs)

## Quick HTTP Examples
```http
POST /Wallet
{ "name": "Primary Wallet" }

POST /Wallet/1/deposit
{ "currencyCode": "USD", "amount": 100 }

POST /Wallet/1/exchange
{ "fromCurrencyCode": "USD", "toCurrencyCode": "EUR", "amount": 50 }

GET /Wallet/1/transactions
```

## Tech Stack
.NET 8, ASP.NET Core, EF Core (Sqlite), MediatR, AutoMapper, FluentValidation, Hosted Service (BackgroundService), HttpClient (NBP API), Swagger, MemoryCache.

## License
Internal / unspecified.