using Microsoft.EntityFrameworkCore;
using System_Uznawania_Przychodow.Data;
using System_Uznawania_Przychodow.Exceptions;
using System_Uznawania_Przychodow.Models;
using System_Uznawania_Przychodow.Requests;
using BadHttpRequestException = Microsoft.AspNetCore.Server.IIS.BadHttpRequestException;

namespace System_Uznawania_Przychodow.Services;

public class KlientService : IKlientService
{
    
    private readonly AppDbContext _context;

    public KlientService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task CreateIndividualAsync(CreateIndividualRequest request)
    {
        var client = await _context.Klients
            .Include(i => i.OsobaFizyczna)
            .FirstOrDefaultAsync(p => p.OsobaFizyczna.Pesel == request.Pesel);

        if (client != null)
        {
            throw new ClientHasExistsException("Klient o podanym numerze PESEL już istnieje!");
        }
        
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            
            var newId = await _context.Klients.MaxAsync(p => p.IdKlient) + 1;
        
            client = new Klient()
            {
                IdKlient = newId,
                Email = request.Email,
                Adres = request.Adres,
                NrTelefonu = request.NrTelefonu,
            };
        
            await _context.Klients.AddAsync(client);
            await _context.SaveChangesAsync();

            var osoba = new OsobaFizyczna()
            {
                IdKlient = newId,
                Pesel = request.Pesel,
                Imie = request.Imie,
                Nazwisko = request.Nazwisko,
                IdKlientNavigation = client
            };
            await _context.OsobaFizycznas.AddAsync(osoba);
            await _context.SaveChangesAsync();
            
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task CreateFirmaAsync(CreateFirmaRequest request)
    {
        var firma = await _context.Firmas.FirstOrDefaultAsync(f => f.Krs == request.KRS);
        if (firma != null)
        {
            throw new ClientHasExistsException("Firma o podanym numerze KRS już istnieje!");
        }
        
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            
            var newId = await _context.Klients.MaxAsync(k => k.IdKlient) + 1;

            var klient = new Klient()
            {
                IdKlient = newId,
                Email = request.Email,
                Adres = request.Adres,
                NrTelefonu = request.NrTelefonu,
            };
            
            await _context.Klients.AddAsync(klient);
            await _context.SaveChangesAsync();

            firma = new Firma()
            {
                IdKlient = newId,
                Krs = request.KRS,
                Nazwa = request.Nazwa
            };
            
            await _context.Firmas.AddAsync(firma);
            await _context.SaveChangesAsync();
            
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
        
    }


    
    
    public async Task UpdateClientAsync(UpdateClientRequest request, int idClient)
    {
        var client = await _context.Klients.FindAsync(idClient);
        if (client == null)
        {
            throw new KeyNotFoundException($"Nie znaleziono klienta indywidualnego o id: {idClient}");
        }

        if (request.NrTelefonu == null && request.Email == null && request.Adres == null)
        {
            throw new UpdateClientException("Chociażby jedno pole musi być zmienione!");
        }

        if(request.NrTelefonu != null) client.NrTelefonu = request.NrTelefonu;
        if(request.Adres != null) client.Adres = request.Adres;
        if(request.Email != null) client.Email = request.Email;
        
        await _context.SaveChangesAsync();
    }


    public async Task DeleteClient(int idClient)
    {
        var client = await _context.OsobaFizycznas.FindAsync(idClient);
        if (client == null)
        {
            throw new KeyNotFoundException($"Nie znaleziono klienta indywidualnego o id: {idClient}");
        }
        
        client.IsDeleted = 1;
        await _context.SaveChangesAsync();
    }
}