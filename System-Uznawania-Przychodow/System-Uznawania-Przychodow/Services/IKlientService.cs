using Microsoft.AspNetCore.Identity.Data;
using System_Uznawania_Przychodow.Requests;
using LoginRequest = System_Uznawania_Przychodow.Requests.LoginRequest;

namespace System_Uznawania_Przychodow.Services;

public interface IKlientService
{
    Task CreateIndividualAsync(CreateIndividualRequest request);
    Task CreateFirmaAsync(CreateFirmaRequest request);
    Task UpdateClientAsync(UpdateClientRequest request, int idClient);
    Task DeleteClient(int idClient);
}