using AutoMapper;
using EventTicketing.Data.Entities.Events;
using EventTicketing.Data.Entities.TicketSales;
using EventTicketing.DTOs.Events;
using EventTicketing.DTOs.TicketSales;

namespace EventTicketing.Data.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Event, EventDto>();

            CreateMap<TicketSale, TicketSalesDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.EventId))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => "Standard"))
                .ForMember(dest => dest.QuantityAvailable, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.QuantitySold, opt => opt.MapFrom(src => 1)); // Each record represents 1 ticket sold

            CreateMap<TicketSalesDto, TicketSale>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.EventId))
                .ForMember(dest => dest.PriceInCents, opt => opt.MapFrom(src => (int)(src.Price * 100)))
                .ForMember(dest => dest.Price, opt => opt.Ignore()) // Price is computed from PriceInCents
                .ForMember(dest => dest.Event, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.PurchaseDate, opt => opt.Ignore());

            CreateMap<Event, EventSalesDto>()
                .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.TicketsSold, opt => opt.MapFrom(src =>
                    src.TicketSale != null ? src.TicketSale.Count() : 0))
                .ForMember(dest => dest.TotalRevenue, opt => opt.MapFrom(src =>
                    src.TicketSale != null ? src.TicketSale.Sum(t => t.Price) : 0m));
        }
    }
}