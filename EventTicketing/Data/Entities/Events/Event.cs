using EventTicketing.Data.Entities.TicketSales;

namespace EventTicketing.Data.Entities.Events
{
    public class Event
    {
        public virtual string Id { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual string Description { get; set; }
        public virtual string Location { get; set; }
        public virtual ICollection<TicketSale> TicketSale
        {
            get; set;
        } = new List<TicketSale>();
    }
}
