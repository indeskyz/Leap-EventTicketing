namespace EventTicketing.DTOs.TicketSales
{
    public class TicketSalesDto
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }          
        public required string Type { get; set; }  
        public decimal Price { get; set; }         
        public int QuantityAvailable { get; set; }
        public int QuantitySold { get; set; }
    }
}
