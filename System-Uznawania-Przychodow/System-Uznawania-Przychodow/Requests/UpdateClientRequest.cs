using System.ComponentModel.DataAnnotations;

namespace System_Uznawania_Przychodow.Requests;

public class UpdateClientRequest
{
    [MaxLength(50)]
    public string? Email { get; set; }
    
    public string? Adres { get; set; }
    
    [MaxLength(20)]
    public string? NrTelefonu { get; set; }
}