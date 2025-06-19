using System.ComponentModel.DataAnnotations;

namespace System_Uznawania_Przychodow.Requests;

public class CreateFirmaRequest
{
    [Required]
    [MaxLength(100)]
    public string Nazwa { get; set; }
    
    [Required]
    [MaxLength(10)]
    [MinLength(10)]
    public string KRS { get; set; }
    
    [Required]
    public string Adres { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Email { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string NrTelefonu { get; set; }
}