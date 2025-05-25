using EventTicketing.Data.Entities.Tickets;

namespace EventTicketing.Data.Entities.Events
{
    public class Event
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual string Description { get; set; }
        public virtual string Location { get; set; }
        public virtual ICollection<Ticket> Tickets
        {
            get; set;
        }
    }
}
