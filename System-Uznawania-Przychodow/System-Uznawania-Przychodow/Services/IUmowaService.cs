using System_Uznawania_Przychodow.DTOs;
using System_Uznawania_Przychodow.Requests;

namespace System_Uznawania_Przychodow.Services;

public interface IUmowaService
{
    Task CreateUmowaAsync(CreateUmowaRequest request);
    Task<BillingContractDTO> BillingContract(int idUmowa);
}