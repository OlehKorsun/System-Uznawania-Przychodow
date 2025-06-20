using System.Text.Json;
using System.Text.Json.Serialization;

namespace System_Uznawania_Przychodow.Services;

public class CurrencyService : ICurrencyService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://v6.exchangerate-api.com/v6/22672882ddc8a6401449f976/pair/PLN";

    public CurrencyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<decimal> GetExchangeRate(string targetCurrencyCode)
    {
        var url = $"{BaseUrl}/{targetCurrencyCode}";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Błąd pobierania kursu waluty.");
        }

        var json = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<ExchangeRateResponse>(json);
        
        Console.WriteLine(json);
        Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-");
        Console.WriteLine(data.Result);

        if (data == null || data?.Result != "success")
        {
            throw new Exception("Nie udało się pobrać poprawnych danych.");
        }

        return data.ConversionRate;
    }

    private class ExchangeRateResponse
    {
        [JsonPropertyName("result")]
        public string Result { get; set; }

        [JsonPropertyName("base_code")]
        public string BaseCode { get; set; }

        [JsonPropertyName("target_code")]
        public string TargetCode { get; set; }

        [JsonPropertyName("conversion_rate")]
        public decimal ConversionRate { get; set; }
    }
}