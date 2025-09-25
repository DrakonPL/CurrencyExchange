using System.Text.Json.Serialization;

namespace CurrencyExchange.Application.Worker
{
    public record NbpRate(
        [property: JsonPropertyName("currency")] string Currency,
        [property: JsonPropertyName("code")] string Code,
        [property: JsonPropertyName("mid")] decimal Mid
    );
}
