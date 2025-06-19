using System.ComponentModel.DataAnnotations;

namespace System_Uznawania_Przychodow.Requests;

public class CreateIndividualRequest
{
    [Required]
    [MaxLength(50)]
    public string Imie { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Nazwisko { get; set; }
    
    [Required]
    [MaxLength(11)]
    [MinLength(11)]
    public string Pesel { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Email { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string NrTelefonu { get; set; }
    
    [Required]
    public string Adres { get; set; }
}