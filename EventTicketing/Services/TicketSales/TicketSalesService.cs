using AutoMapper;
using EventTicketing.Data.Entities.TicketSales;
using EventTicketing.Data.Repositories.Tickets;
using EventTicketing.DTOs;
using EventTicketing.DTOs.Events;
using EventTicketing.DTOs.Pagination;
using EventTicketing.DTOs.TicketSales;
using EventTicketing.Services.Base;

namespace EventTicketing.Services.Tickets
{
    public class TicketSalesService(ITicketSalesRepository repository, IMapper mapper)
     : BaseService<TicketSale, TicketSalesDto, ITicketSalesRepository>(repository, mapper), ITicketSalesService
    {
        public async Task<PagedResult<TicketSalesDto>> GetTicketsForEventAsync(string eventId, PaginationRequest request)
        {
            var (tickets, totalCount) = await _repository.GetTicketsForEventAsync(eventId, request.PageNumber, request.PageSize);

            return new PagedResult<TicketSalesDto>
            {
                Items = _mapper.Map<IEnumerable<TicketSalesDto>>(tickets),
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<ApiResponse<IEnumerable<EventSalesDto>>> GetTopEventsByTicketCountAsync(int count = PaginationConstants.DefaultTopCount)
        {
            count = Math.Min(count, PaginationConstants.MaxTopCount);
            var events = await _repository.GetTopEventsByTicketCountAsync(count);
            var result = _mapper.Map<IEnumerable<EventSalesDto>>(events);

            return new ApiResponse<IEnumerable<EventSalesDto>>(result);
        }

        public async Task<ApiResponse<IEnumerable<EventSalesDto>>> GetTopEventsByRevenueAsync(int count = PaginationConstants.DefaultTopCount)
        {
            count = Math.Min(count, PaginationConstants.MaxTopCount);
            var events = await _repository.GetTopEventsByRevenueAsync(count);
            var result = _mapper.Map<IEnumerable<EventSalesDto>>(events);

            return new ApiResponse<IEnumerable<EventSalesDto>>(result);
        }


    }
}
