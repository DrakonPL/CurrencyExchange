using CurrencyExchange.Application.Worker;
using System.Net.Http.Json;

namespace CurrencyExchange.Infrastructure.Worker
{
    public class NbpClient
    {
        private readonly HttpClient _http;
        public NbpClient(HttpClient http) => _http = http;

        public async Task<NbpTable[]> GetTableBAsync(CancellationToken ct)
        {
            using var resp = await _http.GetAsync("/api/exchangerates/tables/B?format=json", ct);
            resp.EnsureSuccessStatusCode();
            var json = await resp.Content.ReadFromJsonAsync<NbpTable[]>(cancellationToken: ct)
                       ?? throw new InvalidOperationException("NBP JSON null");
            return json;
        }
    }
}
