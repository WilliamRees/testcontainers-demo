namespace SqlIntegrationTesting;

public class Order
{
    public int Id { get; set; }
    
    public string ExternalId { get; set; }
    
    public decimal Amount { get; set; }
    
    public string ProductUpc { get; set; }
}