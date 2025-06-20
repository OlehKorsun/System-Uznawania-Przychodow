using System.Globalization;

namespace System_Uznawania_Przychodow.Tests;

using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;
using System_Uznawania_Przychodow.Services;

public class CurrencyServiceTests
{
    [Fact]
    public async Task GetExchangeRate_ShouldReturnRate_WhenResponseIsSuccessful()
    {
        
        var expectedRate = 4.25m;
        var jsonResponse = @$"{{
            ""result"": ""success"",
            ""base_code"": ""PLN"",
            ""target_code"": ""USD"",
            ""conversion_rate"": {expectedRate.ToString(CultureInfo.InvariantCulture)}
        }}";

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
               "SendAsync",
               ItExpr.IsAny<HttpRequestMessage>(),
               ItExpr.IsAny<CancellationToken>())
           .ReturnsAsync(new HttpResponseMessage()
           {
               StatusCode = HttpStatusCode.OK,
               Content = new StringContent(jsonResponse),
           });

        var httpClient = new HttpClient(handlerMock.Object);

        var service = new CurrencyService(httpClient);
        
        var rate = await service.GetExchangeRate("USD");
        
        Assert.Equal(expectedRate, rate);
    }

    [Fact]
    public async Task GetExchangeRate_ShouldThrowException_WhenResponseIsNotSuccessful()
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
               "SendAsync",
               ItExpr.IsAny<HttpRequestMessage>(),
               ItExpr.IsAny<CancellationToken>())
           .ReturnsAsync(new HttpResponseMessage()
           {
               StatusCode = HttpStatusCode.InternalServerError,
           });

        var httpClient = new HttpClient(handlerMock.Object);
        var service = new CurrencyService(httpClient);
        
        await Assert.ThrowsAsync<Exception>(() => service.GetExchangeRate("USD"));
    }

    [Fact]
    public async Task GetExchangeRate_ShouldThrowException_WhenResultIsNotSuccess()
    {
        var jsonResponse = @$"{{
            ""result"": ""error"",
            ""base_code"": ""PLN"",
            ""target_code"": ""USD"",
            ""conversion_rate"": 0
        }}";

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
               "SendAsync",
               ItExpr.IsAny<HttpRequestMessage>(),
               ItExpr.IsAny<CancellationToken>())
           .ReturnsAsync(new HttpResponseMessage()
           {
               StatusCode = HttpStatusCode.OK,
               Content = new StringContent(jsonResponse),
           });

        var httpClient = new HttpClient(handlerMock.Object);
        var service = new CurrencyService(httpClient);
        
        await Assert.ThrowsAsync<Exception>(() => service.GetExchangeRate("USD"));
    }
}
