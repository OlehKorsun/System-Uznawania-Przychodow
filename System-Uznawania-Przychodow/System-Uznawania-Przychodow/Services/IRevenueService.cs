using System_Uznawania_Przychodow.DTOs;
using System_Uznawania_Przychodow.Requests;

namespace System_Uznawania_Przychodow.Services;

public interface IRevenueService
{
    Task<PrzychodDTO> GetPrzychod(PrzychodRequest przychodRequest);
    Task<PrzychodDTO> GetPrzychodPrzewidywalny(PrzychodRequest przychodRequest);
}