using System_Uznawania_Przychodow.Requests;
using LoginRequest = System_Uznawania_Przychodow.Requests.LoginRequest;

namespace System_Uznawania_Przychodow.Services;

public interface IAuthService
{
    Task<String> LoginAsync(LoginRequest dto);
    Task RegisterAsync(RegisterUserRequest dto);
}