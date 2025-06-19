namespace System_Uznawania_Przychodow.Services;

public interface ICurrencyService
{
    Task<decimal> GetExchangeRate(string targetCurrencyCode);
}