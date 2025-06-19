using System.Text.Json;

namespace System_Uznawania_Przychodow.Services;

public class CurrencyService : ICurrencyService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://v6.exchangerate-api.com/v6/22672882ddc8a6401449f976/pair/PLN/";

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

        if (data?.Result != "success")
        {
            throw new Exception("Nie udało się pobrać poprawnych danych.");
        }

        return data.Conversion_Rate;
    }

    private class ExchangeRateResponse
    {
        public string Result { get; set; }
        public string Base_Code { get; set; }
        public string Target_Code { get; set; }
        public decimal Conversion_Rate { get; set; }
        
    }
}