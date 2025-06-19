using System.ComponentModel.DataAnnotations;

namespace System_Uznawania_Przychodow.Requests;

public class CreateUmowaRequest
{
    [Required]
    public DateOnly DataOd { get; set; }
    
    [Required]
    public DateOnly DataDo { get; set; }
    
    [Required]
    public double Cena { get; set; }
    
    [Required]
    public int IdSprzedawca { get; set; }
    
    [Required]
    public int IdOdbiorca { get; set; }
    
    [Required]
    public int IdOprogramowanie { get; set; }
    
    public RataRequest? Rata { get; set; }
    
    public int? IdZnizka  { get; set; }
}

public class RataRequest
{
    [Required]
    public int Wartosc { get; set; }
    
    [Required]
    public int DzienMiesiaca { get; set; }
}