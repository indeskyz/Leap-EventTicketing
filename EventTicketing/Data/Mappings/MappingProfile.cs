using AutoMapper;
using EventTicketing.Data.Entities.Events;
using EventTicketing.Data.Entities.Tickets;
using EventTicketing.DTOs.Events;
using EventTicketing.DTOs.Tickets;

namespace EventTicketing.Data.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Event, EventDto>();
            CreateMap<Ticket, TicketDto>();
            CreateMap<Event, EventSalesDto>()
                .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.TicketsSold, opt => opt.MapFrom(src => src.Tickets.Sum(t => t.QuantitySold)))
                .ForMember(dest => dest.TotalRevenue, opt => opt.MapFrom(src => src.Tickets.Sum(t => t.Price * t.QuantitySold)));
        }
    }
}
