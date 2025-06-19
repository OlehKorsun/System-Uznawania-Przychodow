using Microsoft.EntityFrameworkCore;
using System_Uznawania_Przychodow.Data;
using System_Uznawania_Przychodow.DTOs;
using System_Uznawania_Przychodow.Models;
using System_Uznawania_Przychodow.Requests;

namespace System_Uznawania_Przychodow.Services;

public class RevenueService : IRevenueService
{
    
    private readonly AppDbContext _context;
    private readonly ICurrencyService _currencyService;

    public RevenueService(AppDbContext context, ICurrencyService currencyService)
    {
        _context = context;
        _currencyService = currencyService;
    }
    public async Task<PrzychodDTO> GetPrzychod(PrzychodRequest przychodRequest)
    {
        IQueryable<Umowa> umowy = _context.Umowas;

        if (przychodRequest.IdOprogramowania != null)
            umowy = umowy.Where(u => u.IdOprogramowanie == przychodRequest.IdOprogramowania);

        decimal przychod = 0;

        var oplaconeUmowy = await umowy
            .Where(u => u.CzyOplacona == 1)
            .ToListAsync();

        foreach (Umowa umowa in oplaconeUmowy)
        {
            przychod += umowa.Cena;
        }
        
        string waluta = "PLN";

        if (!string.IsNullOrEmpty(przychodRequest.WalutaDocelowa) && przychodRequest.WalutaDocelowa != "PLN")
        {
            var kurs = await _currencyService.GetExchangeRate(przychodRequest.WalutaDocelowa);
            przychod *= kurs;
            waluta = przychodRequest.WalutaDocelowa.ToUpper();
        }

        var result = new PrzychodDTO()
        {
            Wartosc = Math.Round(przychod, 2),
            Waluta = waluta
        };

        return result;
    }

    public async Task<PrzychodDTO> GetPrzychodPrzewidywalny(PrzychodRequest przychodRequest)
    {
        IQueryable<Umowa> umowy = _context.Umowas;

        if (przychodRequest.IdOprogramowania != null)
            umowy = umowy.Where(u => u.IdOprogramowanie == przychodRequest.IdOprogramowania);

        decimal przychod = 0;
        

        foreach (Umowa umowa in umowy)
        {
            przychod += umowa.Cena;
        }
        
        string waluta = "PLN";

        if (!string.IsNullOrEmpty(przychodRequest.WalutaDocelowa) && przychodRequest.WalutaDocelowa != "PLN")
        {
            var kurs = await _currencyService.GetExchangeRate(przychodRequest.WalutaDocelowa);
            przychod *= kurs;
            waluta = przychodRequest.WalutaDocelowa.ToUpper();
        }

        var result = new PrzychodDTO()
        {
            Wartosc = Math.Round(przychod, 2),
            Waluta = waluta
        };

        return result;
    }
}