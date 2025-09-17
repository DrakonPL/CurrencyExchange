# CurrencyExchange

Minimal wallet-based currency exchange API (.NET 8, EF Core Sqlite, MediatR, AutoMapper, FluentValidation).

## Architecture
Projects:
- Api: HTTP layer + Swagger + error middleware ([src/CurrencyExchange.Api](src/CurrencyExchange.Api))
- Application: CQRS (MediatR), DTOs, validation, background rates worker
- Infrastructure: EF Core repositories, Sqlite context
- Domain: Entities (Wallet, Funds, Currency)
- Common: Shared exceptions
- UnitTests: In-memory tests

Key components:
- Controller: [`CurrencyExchange.Api.Controllers.WalletController`](src/CurrencyExchange.Api/Controllers/WalletController.cs), [`CurrencyExchange.Api.Controllers.CurrencyController`](src/CurrencyExchange.Api/Controllers/CurrencyController.cs)
- Handlers: e.g. [`CurrencyExchange.Application.Features.Funds.Commands.DepositFunds.DepositFundsHandler`](src/CurrencyExchange.Application/Features/Funds/Commands/DepositFunds/DepositFundsHandler.cs)
- Rates worker: [`CurrencyExchange.Application.Worker.RatesWorker`](src/CurrencyExchange.Application/Worker/RatesWorker.cs) + [`CurrencyExchange.Application.Worker.NbpClient`](src/CurrencyExchange.Application/Worker/NbpClient.cs)
- Conversion service: [`CurrencyExchange.Application.Services.CurrencyConverter`](src/CurrencyExchange.Application/Services/CurrencyConverter.cs)
- DbContext: [`CurrencyExchange.Infrastructure.CurrencyExchangeDbContext`](src/CurrencyExchange.Infrastructure/CurrencyExchangeDbContext.cs)
- Error middleware: [`CurrencyExchange.Api.Middlewares.ErrorHandlerMiddleware`](src/CurrencyExchange.Api/Middlewares/ErrorHandlerMiddleware.cs)

## Data Model
Entities: [`Currency`](src/CurrencyExchange.Domain/Entities/Currency.cs) (Code, Name, Rate PLN-based), [`Wallet`](src/CurrencyExchange.Domain/Entities/Wallet.cs) (Id, Name, Funds), [`Funds`](src/CurrencyExchange.Domain/Entities/Funds.cs) (CurrencyId, Amount). All currency conversion assumes PLN as implicit base: amount_in_target = (amount * from.Rate) / to.Rate.

## Background Rates
[`RatesWorker`](src/CurrencyExchange.Application/Worker/RatesWorker.cs) fetches NBP Table B every 4h and inserts/updates currencies via [`NbpClient`](src/CurrencyExchange.Application/Worker/NbpClient.cs). No initial seed beyond what the worker loads at startup (first run triggers immediately). Add manual seeding if you need deterministic startup currencies.

## Endpoints
Base route style: /{ControllerName}
- GET /Currency → list all currencies
- GET /Wallet → list all wallets with funds
- POST /Wallet (body: { "name": "My Wallet" }) → creates wallet (returns new id)
- POST /Wallet/deposit (body: { walletId, currencyCode, amount })
- POST /Wallet/withdraw (body: { walletId, currencyCode, amount })
- POST /Wallet/exchange (body: { walletId, fromCurrencyCode, toCurrencyCode, amount })
(There is a query handler [`CurrencyExchange.Application.Features.Wallet.Queries.GetWallet.GetWalletHandler`](src/CurrencyExchange.Application/Features/Wallet/Queries/GetWallet/GetWalletHandler.cs) but no GET /Wallet/{id} endpoint yet.)

Sample requests file: [src/CurrencyExchange.Api/CurrencyExchange.Api.http](src/CurrencyExchange.Api/CurrencyExchange.Api.http)

## Validation & Errors
FluentValidation validators (e.g. [`DepositFundsDtoValidator`](src/CurrencyExchange.Application/DTOs/Funds/Validators/DepositFundsDtoValidator.cs)) enforce existence and positive amounts. Errors map to HTTP codes via [`ErrorHandlerMiddleware`](src/CurrencyExchange.Api/Middlewares/ErrorHandlerMiddleware.cs):
- 400 BadRequestException / validation failures
- 404 EntityNotFoundException (custom)
- 500 unhandled

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
Database file stored in local app data (see [`CurrencyExchangeDbContext`](src/CurrencyExchange.Infrastructure/CurrencyExchangeDbContext.cs)).

## Tests
Run unit tests:
```bash
dotnet test
```
Tests use InMemory provider (see [`TestFixture`](tests/CurrencyExchange.UnitTests/TestFixture.cs)) and cover handlers + validators + currency conversion.

## Future Improvements (optional)
- Add paging
- Add optimistic concurrency on funds
- Add seeding or base PLN currency guarantee
- Add authentication/authorization

## Quick HTTP Examples
```http
POST /Wallet
{ "name": "Primary Wallet" }

POST /Wallet/deposit
{ "walletId": 1, "currencyCode": "USD", "amount": 100 }

POST /Wallet/exchange
{ "walletId": 1, "fromCurrencyCode": "USD", "toCurrencyCode": "EUR", "amount": 50 }
```

## Tech Stack
.NET 8, ASP.NET Core, EF Core (Sqlite), MediatR, AutoMapper, FluentValidation, Hosted Service (BackgroundService), HttpClient (NBP API), Swagger.

## License
Internal / unspecified.