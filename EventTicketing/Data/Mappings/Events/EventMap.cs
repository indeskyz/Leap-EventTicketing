using EventTicketing.Data.Entities.Events;
using FluentNHibernate.Mapping;

namespace EventTicketing.Data.Mappings.Events
{
    public class EventMap : ClassMap<Event>
    {
        public EventMap()
        {
            Table("Events");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name).Not.Nullable().Length(100);
            Map(x => x.StartsOn).Not.Nullable();
            Map(x => x.EndsOn).Not.Nullable();
            Map(x => x.Description).Nullable().Length(500);
            Map(x => x.Location).Nullable().Length(100);
            HasMany(x => x.Tickets)
                .KeyColumn("EventId")
                .Inverse()
                .Cascade.AllDeleteOrphan();
        }
    }
}
