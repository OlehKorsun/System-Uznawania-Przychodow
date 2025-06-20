using Microsoft.EntityFrameworkCore;
using System_Uznawania_Przychodow.Data;
using System_Uznawania_Przychodow.Exceptions;
using System_Uznawania_Przychodow.Requests;
using System_Uznawania_Przychodow.Services;

namespace System_Uznawania_Przychodow.Tests;

public class UmowaServiceTests
{
    private AppDbContext GetDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);

        return context;
    }

    [Fact]
    public async Task CreateUmowaAsync_ShouldThrowDateException_WhenDurationIsTooShort()
    {
        var context = GetDbContext(nameof(CreateUmowaAsync_ShouldThrowDateException_WhenDurationIsTooShort));
        var service = new UmowaService(context);

        var request = new CreateUmowaRequest
        {
            DataOd = new DateOnly(2025, 6, 1),
            DataDo = new DateOnly(2025, 6, 2), // tylko 1 dzień
            IdOdbiorca = 1,
            IdOprogramowanie = 1,
            Cena = 1000,
        };
        
        var exception = await Assert.ThrowsAsync<DateException>(() => service.CreateUmowaAsync(request));
        Assert.Equal("Czas trwania umowy musi wynosić co najmniej 3 dni i maksymalnie 30 dni!", exception.Message);
    }
}