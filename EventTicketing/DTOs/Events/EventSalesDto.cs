namespace EventTicketing.DTOs.Events
{
    public class EventSalesDto
    {
        public Guid EventId { get; set; }
        public string EventName { get; set; }
        public int TicketsSold { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
