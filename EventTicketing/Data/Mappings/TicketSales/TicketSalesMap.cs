using EventTicketing.Data.Entities.TicketSales;
using FluentNHibernate.Mapping;

namespace EventTicketing.Data.Mappings.Tickets
{
    public class TicketSaleMap : ClassMap<TicketSale>
    {
        public TicketSaleMap()
        {
            Table("TicketSales");

            Id(x => x.Id)
                .GeneratedBy.Assigned()
                .Column("Id")
                .CustomType("Guid");

            References(x => x.Event)
                .Column("EventId")
                .Not.Nullable();

            Map(x => x.EventId)
                .Column("EventId")
                .Not.Insert()
                .Not.Update()
                .CustomType("Guid");

            Map(x => x.UserId)
                .Column("UserId")
                .Not.Nullable()
                .CustomType("Guid");

            Map(x => x.PurchaseDate)
                .Column("PurchaseDate")
                .Not.Nullable();

            Map(x => x.PriceInCents)
                .Column("PriceInCents")
                .Not.Nullable();
        }
    }
}
