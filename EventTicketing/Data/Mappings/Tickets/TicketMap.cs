using EventTicketing.Data.Entities.Tickets;
using FluentNHibernate.Mapping;

namespace EventTicketing.Data.Mappings.Tickets
{
    public class TicketMap : ClassMap<Ticket>
    {
        public TicketMap()
        {
            Table("Tickets");
            Id(x => x.Id).GeneratedBy.Identity();
            References(x => x.Event).Column("EventId").Not.Nullable();
            Map(x => x.Type).Not.Nullable().Length(50);
            Map(x => x.Price).Not.Nullable().CustomType("decimal(10,2)");
            Map(x => x.QuantitySold).Not.Nullable();
        }
    }
}
