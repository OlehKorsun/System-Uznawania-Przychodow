using Microsoft.EntityFrameworkCore;
using System_Uznawania_Przychodow.Data;
using System_Uznawania_Przychodow.DTOs;
using System_Uznawania_Przychodow.Exceptions;
using System_Uznawania_Przychodow.Models;
using System_Uznawania_Przychodow.Requests;

namespace System_Uznawania_Przychodow.Services;

public class UmowaService : IUmowaService
{
    private readonly AppDbContext _context;

    public UmowaService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task CreateUmowaAsync(CreateUmowaRequest request)
    {
        if ((request.DataDo.DayNumber - request.DataOd.DayNumber) < 3 ||
            (request.DataDo.DayNumber - request.DataOd.DayNumber) > 30)
        {
            throw new DateException("Czas trwania umowy musi wynosić co najmniej 3 dni i maksymalnie 30 dni!");
        }

        var client = await _context.Umowas.FirstOrDefaultAsync(u => u.Odbiorca == request.IdOdbiorca && u.IdOprogramowanie == request.IdOprogramowanie);

        if (client != null)
        {
            throw new ClientHasContractException($"Client o id: {request.IdOdbiorca} już posiada umowę na oprogramowanie z id: {request.IdOprogramowanie}!");
        }
        
        var newId = await _context.Umowas.MaxAsync(a => a.IdUmowa)+1;

        Znizka znizkaClient = await _context.Znizkas.FindAsync(request.IdZnizka);
        Znizka znizkaStaly = null;
        
        int? znizkaId = null;

        if (await _context.Umowas.FirstOrDefaultAsync(u => u.Odbiorca == request.IdOdbiorca) != null)
            znizkaStaly = await _context.Znizkas.FirstOrDefaultAsync(z => z.Nazwa == "Dla stalych klientow");

        if (znizkaStaly != null && znizkaClient != null)
        {
            if(znizkaStaly.Wartosc > znizkaClient.Wartosc)
                znizkaId = znizkaStaly.IdZnizka;
            else 
                znizkaId = znizkaClient.IdZnizka;
        }
        else if(znizkaStaly != null)
        {
            znizkaId = znizkaStaly.IdZnizka;
        }
        else if (znizkaClient != null)
        {
            znizkaId = znizkaClient.IdZnizka;
        }
        
        Umowa umowa = new Umowa()
        {
            IdUmowa = newId,
            IdOprogramowanie = request.IdOprogramowanie,
            Sprzedawca = request.IdSprzedawca,
            Odbiorca = request.IdOdbiorca,
            DataOd = request.DataOd,
            DataDo = request.DataDo,
            Cena = request.Cena,
            CzyOplacona = 0,
            CzyPodpisana = 0,
            IdZnizka = znizkaId,
        };
        
        await _context.Umowas.AddAsync(umowa);
        await _context.SaveChangesAsync();
    }


    public async Task<BillingContractDTO> BillingContract(int idUmowa)
    {
        var umowa = await _context.Umowas
            .Include(u => u.IdOprogramowanieNavigation)
            .Include(u => u.IdRataNavigation)
            .FirstOrDefaultAsync(u => u.IdUmowa == idUmowa);
        
        if (umowa == null)
            throw new KeyNotFoundException($"Nie znaleziono umowy o id: {idUmowa}!");
        
        
        var klient = await _context.Klients
            .Include(k => k.OsobaFizyczna)
            .Include(k => k.Firma)
            .FirstOrDefaultAsync(k => k.IdKlient == umowa.Odbiorca);
        
        if (klient == null)
            throw new KeyNotFoundException($"Nie znaleziono klienta o id:{umowa.Odbiorca}!");
        
        
        var imieLubNazwa = klient.OsobaFizyczna != null
            ? $"{klient.OsobaFizyczna.Imie} {klient.OsobaFizyczna.Nazwisko}"
            : klient.Firma?.Nazwa ?? "(Nieznany odbiorca)";


        var result = new BillingContractDTO()
        {
            IdUmowa = umowa.IdUmowa,
            Odbiorca = imieLubNazwa,
            Oprogramowanie = (await _context.Oprogramowanies.FindAsync(umowa.IdOprogramowanie))?.Nazwa,
            DzienMiesiaca = umowa.IdRata != null
                ? (await _context.Rata.FindAsync(umowa.IdRata))?.DzienMiesiaca
                : null,
            Wartosc = umowa.Cena,
        };
        
        return result;
    }
}