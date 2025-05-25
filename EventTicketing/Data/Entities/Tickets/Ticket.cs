using EventTicketing.Data.Entities.Events;

namespace EventTicketing.Data.Entities.Tickets
{
    public class Ticket
    {
        public virtual int Id { get; set; }
        public virtual required Event Event { get; set; }
        public virtual required string Type { get; set; }
        public virtual decimal Price { get; set; }
        public virtual int QuantityAvailable { get; set; }
        public virtual int QuantitySold { get; set; }
    }

}
