using System.ComponentModel.DataAnnotations;

namespace System_Uznawania_Przychodow.Requests;

public class RegisterUserRequest
{
    [Required]
    [MaxLength(50)]
    public string Login { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Password { get; set; }
    
    
    [MaxLength(50)]
    public string? Rola { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Email { get; set; }
}