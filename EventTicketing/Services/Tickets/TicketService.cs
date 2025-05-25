using AutoMapper;
using EventTicketing.Data.Entities.Tickets;
using EventTicketing.Data.Repositories.Tickets;
using EventTicketing.DTOs.Events;
using EventTicketing.DTOs.Pagination;
using EventTicketing.DTOs.Tickets;
using EventTicketing.Services.Base;

namespace EventTicketing.Services.Tickets
{
    public class TicketService(ITicketRepository repository, IMapper mapper)
     : BaseService<Ticket, TicketDto, ITicketRepository>(repository, mapper), ITicketService
    {
        public async Task<PagedResult<TicketDto>> GetTicketsForEventAsync(int eventId, PaginationRequest request)
        {
            var (tickets, totalCount) = await _repository.GetTicketsForEventAsync(eventId, request.PageNumber, request.PageSize);

            return new PagedResult<TicketDto>
            {
                Items = _mapper.Map<IEnumerable<TicketDto>>(tickets),
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<IEnumerable<EventSalesDto>> GetTopEventsByTicketCountAsync(int count = PaginationConstants.DefaultTopCount)
        {
            count = Math.Min(count, PaginationConstants.MaxTopCount);
            var events = await _repository.GetTopEventsByTicketCountAsync(count);
            return _mapper.Map<IEnumerable<EventSalesDto>>(events);
        }

        public async Task<IEnumerable<EventSalesDto>> GetTopEventsByRevenueAsync(int count = PaginationConstants.DefaultTopCount)
        {
            count = Math.Min(count, PaginationConstants.MaxTopCount);
            var events = await _repository.GetTopEventsByRevenueAsync(count);
            return _mapper.Map<IEnumerable<EventSalesDto>>(events);
        }
    }
}
