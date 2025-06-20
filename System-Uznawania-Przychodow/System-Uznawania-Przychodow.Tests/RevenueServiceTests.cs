using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using System_Uznawania_Przychodow.Data;
using System_Uznawania_Przychodow.DTOs;
using System_Uznawania_Przychodow.Models;
using System_Uznawania_Przychodow.Requests;
using System_Uznawania_Przychodow.Services;
using Xunit;

namespace System_Uznawania_Przychodow.Tests;

public class RevenueServiceTests
{
    private AppDbContext GetDbContextWithData(List<Umowa> umowy)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "RevenueTestDb_" + System.Guid.NewGuid())
            .Options;
        var context = new AppDbContext(options);
        
        context.Umowas.AddRange(umowy);
        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task GetPrzychod_ShouldReturnSumOfPaidContracts_WhenNoCurrencyConversion()
    {
        var umowy = new List<Umowa>()
        {
            new() {IdUmowa = 1, IdOprogramowanie = 1, Cena = 100m, CzyOplacona = 1 },
            new() {IdUmowa = 2, IdOprogramowanie = 1, Cena = 200m, CzyOplacona = 1 },
            new() {IdUmowa = 3, IdOprogramowanie = 2, Cena = 300m, CzyOplacona = 0 }
        };
        var context = GetDbContextWithData(umowy);

        var currencyMock = new Mock<ICurrencyService>();

        var service = new RevenueService(context, currencyMock.Object);

        var request = new PrzychodRequest { IdOprogramowania = 1, WalutaDocelowa = "PLN" };
        
        var result = await service.GetPrzychod(request);
        
        Assert.Equal(300m, result.Wartosc);
        Assert.Equal("PLN", result.Waluta);
        currencyMock.Verify(x => x.GetExchangeRate(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetPrzychod_ShouldConvertCurrency_WhenTargetCurrencyIsNotPLN()
    {
        var umowy = new List<Umowa>()
        {
            new() {IdUmowa = 1, IdOprogramowanie = 1, Cena = 100m, CzyOplacona = 1 },
            new() {IdUmowa = 2, IdOprogramowanie = 1, Cena = 200m, CzyOplacona = 1 }
        };
        var context = GetDbContextWithData(umowy);

        var currencyMock = new Mock<ICurrencyService>();
        currencyMock.Setup(x => x.GetExchangeRate("USD")).ReturnsAsync(4m);

        var service = new RevenueService(context, currencyMock.Object);

        var request = new PrzychodRequest { IdOprogramowania = 1, WalutaDocelowa = "USD" };
        
        var result = await service.GetPrzychod(request);
        
        Assert.Equal(1200m, result.Wartosc); // (100 + 200) * 4
        Assert.Equal("USD", result.Waluta);
        currencyMock.Verify(x => x.GetExchangeRate("USD"), Times.Once);
    }

    [Fact]
    public async Task GetPrzychodPrzewidywalny_ShouldReturnSumOfAllContracts_WhenNoCurrencyConversion()
    {
        var umowy = new List<Umowa>()
        {
            new() {IdUmowa = 1, IdOprogramowanie = 1, Cena = 100m, CzyOplacona = 1 },
            new() {IdUmowa = 2, IdOprogramowanie = 1, Cena = 200m, CzyOplacona = 0 },
            new() {IdUmowa = 3, IdOprogramowanie = 2, Cena = 300m, CzyOplacona = 1 }
        };
        var context = GetDbContextWithData(umowy);

        var currencyMock = new Mock<ICurrencyService>();

        var service = new RevenueService(context, currencyMock.Object);

        var request = new PrzychodRequest { IdOprogramowania = null, WalutaDocelowa = "PLN" };
        
        var result = await service.GetPrzychodPrzewidywalny(request);
        
        Assert.Equal(600m, result.Wartosc); // 100 + 200 + 300, bo przewidywalny bierze wszystkie
        Assert.Equal("PLN", result.Waluta);
        currencyMock.Verify(x => x.GetExchangeRate(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetPrzychodPrzewidywalny_ShouldConvertCurrency_WhenTargetCurrencyIsNotPLN()
    {
        var umowy = new List<Umowa>()
        {
            new() {IdUmowa = 1, IdOprogramowanie = 1, Cena = 100m },
            new() {IdUmowa = 2, IdOprogramowanie = 1, Cena = 200m },
        };
        var context = GetDbContextWithData(umowy);

        var currencyMock = new Mock<ICurrencyService>();
        currencyMock.Setup(x => x.GetExchangeRate("EUR")).ReturnsAsync(5m);

        var service = new RevenueService(context, currencyMock.Object);

        var request = new PrzychodRequest { IdOprogramowania = 1, WalutaDocelowa = "EUR" };
        
        var result = await service.GetPrzychodPrzewidywalny(request);
        
        Assert.Equal(1500m, result.Wartosc); // (100 + 200) * 5
        Assert.Equal("EUR", result.Waluta);
        currencyMock.Verify(x => x.GetExchangeRate("EUR"), Times.Once);
    }
}
