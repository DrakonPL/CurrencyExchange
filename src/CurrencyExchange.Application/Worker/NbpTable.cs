namespace CurrencyExchange.Application.Worker
{
    public record NbpTable(string table, string no, DateOnly effectiveDate, NbpRate[] rates);
}
