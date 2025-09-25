using System.Text.Json.Serialization;

namespace CurrencyExchange.Application.Worker
{
    public record NbpTable(
        [property: JsonPropertyName("table")] string Table,
        [property: JsonPropertyName("no")] string No,
        [property: JsonPropertyName("effectiveDate")] DateTime EffectiveDate,
        [property: JsonPropertyName("rates")] IReadOnlyList<NbpRate> Rates
    );
}
