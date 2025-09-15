# CurrencyExchange
CurrencyExchange


dotnet ef migrations add InitialCreate --project CurrencyExchange.Infrastructure --startup-project CurrencyExchange.Api --output-dir Migrations
dotnet ef database update --project CurrencyExchange.Infrastructure --startup-project CurrencyExchange.Api