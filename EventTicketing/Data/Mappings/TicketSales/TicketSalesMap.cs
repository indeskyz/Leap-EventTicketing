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
                .CustomType("String");

            References(x => x.Event)
                .Column("EventId")
                .Not.Nullable();

            Map(x => x.EventId)
                .Column("EventId")
                .Not.Insert()
                .Not.Update()
                .CustomType("String");

            Map(x => x.UserId)
                .Column("UserId")
                .Not.Nullable()
                .CustomType("String");

            Map(x => x.PurchaseDate)
                .Column("PurchaseDate")
                .Not.Nullable();

            Map(x => x.Price)
                .Formula("PriceInCents / 100.0")
                .ReadOnly();

            Map(x => x.PriceInCents)
                .Column("PriceInCents")
                .Not.Nullable();
        }
    }
}
