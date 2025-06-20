namespace System_Uznawania_Przychodow.Tests;

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System_Uznawania_Przychodow.Data;
using System_Uznawania_Przychodow.Models;
using System_Uznawania_Przychodow.Requests;
using System_Uznawania_Przychodow.Services;
using System_Uznawania_Przychodow.Exceptions;

public class KlientServiceTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())  
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateIndividualAsync_ShouldCreateNewClient_WhenPeselDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var service = new KlientService(context);

        var request = new CreateIndividualRequest
        {
            Pesel = "12345678901",
            Imie = "Jan",
            Nazwisko = "Kowalski",
            Email = "jan.kowalski@example.com",
            Adres = "ul. Testowa 1",
            NrTelefonu = "123456789"
        };
        
        await service.CreateIndividualAsync(request);
        
        var klient = await context.Klients.Include(k => k.OsobaFizyczna).FirstOrDefaultAsync();
        Assert.NotNull(klient);
        Assert.Equal(request.Email, klient.Email);
        Assert.NotNull(klient.OsobaFizyczna);
        Assert.Equal(request.Pesel, klient.OsobaFizyczna.Pesel);
    }

    [Fact]
    public async Task CreateIndividualAsync_ShouldThrow_WhenPeselExists()
    {
        var context = GetInMemoryDbContext();
        var existingClient = new Klient { IdKlient = 1, Email = "exist@example.com", Adres = "adres", NrTelefonu = "111" };
        var existingOsoba = new OsobaFizyczna { IdKlient = 1, Pesel = "12345678901", Imie = "Adam", Nazwisko = "Nowak", IdKlientNavigation = existingClient };

        context.Klients.Add(existingClient);
        context.OsobaFizycznas.Add(existingOsoba);
        await context.SaveChangesAsync();

        var service = new KlientService(context);

        var request = new CreateIndividualRequest
        {
            Pesel = "12345678901",
            Imie = "Jan",
            Nazwisko = "Kowalski",
            Email = "jan.kowalski@example.com",
            Adres = "ul. Testowa 1",
            NrTelefonu = "123456789"
        };
        
        await Assert.ThrowsAsync<ClientHasExistsException>(() => service.CreateIndividualAsync(request));
    }

    [Fact]
    public async Task CreateFirmaAsync_ShouldCreateNewFirma_WhenKRSDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var service = new KlientService(context);

        var request = new CreateFirmaRequest
        {
            KRS = "9876543210",
            Nazwa = "FirmaTest",
            Email = "firma@example.com",
            Adres = "ul. Biznesowa 2",
            NrTelefonu = "987654321"
        };
        
        await service.CreateFirmaAsync(request);
        
        var firma = await context.Firmas.Include(f => f.IdKlientNavigation).FirstOrDefaultAsync();
        Assert.NotNull(firma);
        Assert.Equal(request.KRS, firma.Krs);
        Assert.Equal(request.Nazwa, firma.Nazwa);
        Assert.NotNull(firma.IdKlientNavigation);
        Assert.Equal(request.Email, firma.IdKlientNavigation.Email);
    }

    [Fact]
    public async Task CreateFirmaAsync_ShouldThrow_WhenKRSExists()
    {
        var context = GetInMemoryDbContext();
        var existingClient = new Klient { IdKlient = 1, Email = "exist@example.com", Adres = "adres", NrTelefonu = "111" };
        var existingFirma = new Firma { IdKlient = 1, Krs = "9876543210", Nazwa = "FirmaExist" };

        context.Klients.Add(existingClient);
        context.Firmas.Add(existingFirma);
        await context.SaveChangesAsync();

        var service = new KlientService(context);

        var request = new CreateFirmaRequest
        {
            KRS = "9876543210",
            Nazwa = "NowaFirma",
            Email = "nowa@example.com",
            Adres = "ul. Nowa 3",
            NrTelefonu = "222333444"
        };
        
        await Assert.ThrowsAsync<ClientHasExistsException>(() => service.CreateFirmaAsync(request));
    }

    [Fact]
    public async Task UpdateClientAsync_ShouldUpdateFields_WhenValidRequest()
    {
        var context = GetInMemoryDbContext();
        var klient = new Klient { IdKlient = 1, Email = "old@example.com", Adres = "stary adres", NrTelefonu = "111111111" };
        context.Klients.Add(klient);
        await context.SaveChangesAsync();

        var service = new KlientService(context);

        var updateRequest = new UpdateClientRequest
        {
            Email = "new@example.com",
            Adres = "nowy adres",
            NrTelefonu = "999999999"
        };
        
        await service.UpdateClientAsync(updateRequest, klient.IdKlient);
        
        var updatedClient = await context.Klients.FindAsync(klient.IdKlient);
        Assert.Equal(updateRequest.Email, updatedClient.Email);
        Assert.Equal(updateRequest.Adres, updatedClient.Adres);
        Assert.Equal(updateRequest.NrTelefonu, updatedClient.NrTelefonu);
    }

    [Fact]
    public async Task UpdateClientAsync_ShouldThrow_WhenClientNotFound()
    {
        var context = GetInMemoryDbContext();
        var service = new KlientService(context);
        var updateRequest = new UpdateClientRequest { Email = "new@example.com" };
        
        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.UpdateClientAsync(updateRequest, 999));
    }

    [Fact]
    public async Task UpdateClientAsync_ShouldThrow_WhenNoFieldsChanged()
    {
        var context = GetInMemoryDbContext();
        var klient = new Klient { IdKlient = 1, Email = "old@example.com", Adres = "adres", NrTelefonu = "111" };
        context.Klients.Add(klient);
        await context.SaveChangesAsync();

        var service = new KlientService(context);
        var updateRequest = new UpdateClientRequest();
        
        await Assert.ThrowsAsync<UpdateClientException>(() => service.UpdateClientAsync(updateRequest, klient.IdKlient));
    }

    [Fact]
    public async Task DeleteClient_ShouldMarkClientAsDeleted_WhenClientExists()
    {
        var context = GetInMemoryDbContext();
        
        var osoba = new OsobaFizyczna {Imie = "Jan", Nazwisko = "Kowalski", IdKlient = 1, Pesel = "12345678901", IsDeleted = 0 };
        context.OsobaFizycznas.Add(osoba);
        await context.SaveChangesAsync();

        var service = new KlientService(context);
        
        await service.DeleteClient(1);
        
        var deletedOsoba = await context.OsobaFizycznas.FindAsync(1);
        Assert.Equal(1, deletedOsoba.IsDeleted);
    }

    [Fact]
    public async Task DeleteClient_ShouldThrow_WhenClientNotFound()
    {
        var context = GetInMemoryDbContext();
        var service = new KlientService(context);
        
        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteClient(999));
    }
}
