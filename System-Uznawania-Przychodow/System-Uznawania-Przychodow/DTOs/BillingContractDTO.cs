namespace System_Uznawania_Przychodow.DTOs;

public class BillingContractDTO
{
    public int IdUmowa { get; set; }
    public string Oprogramowanie { get; set; }
    public string Odbiorca { get; set; }
    public decimal Wartosc { get; set; }
    public int? DzienMiesiaca { get; set; }
}