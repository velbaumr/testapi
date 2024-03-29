namespace WebApi.Entities;

public class Amount
{
    public decimal  Discount { get; set; }
    
    public decimal Paid { get; set; }
    
    public decimal Return { get; set; }
    
    public decimal Total { get; set; }
}