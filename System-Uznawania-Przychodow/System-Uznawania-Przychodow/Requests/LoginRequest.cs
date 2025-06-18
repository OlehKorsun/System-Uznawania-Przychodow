using System.ComponentModel.DataAnnotations;

namespace System_Uznawania_Przychodow.Requests;

public class LoginRequest
{
    [Required]
    [MaxLength(50)]
    public string Login { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Password { get; set; }
}