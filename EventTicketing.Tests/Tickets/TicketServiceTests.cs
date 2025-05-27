using AutoMapper;
using EventTicketing.Cache.Services;
using EventTicketing.Data.Entities.Events;
using EventTicketing.Data.Repositories.Tickets;
using EventTicketing.DTOs.Events;
using EventTicketing.Services.Tickets;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTicketing.Tests.Tickets
{
    public class TicketServiceTests
    {
        private readonly Mock<ITicketSalesRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ICacheService> _mockCacheService;
        private readonly ITicketSalesService _service;

        public TicketServiceTests()
        {
            _mockRepository = new Mock<ITicketSalesRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockCacheService = new Mock<ICacheService>();
            _service = new TicketSalesService(_mockRepository.Object, _mockMapper.Object, _mockCacheService.Object);
        }

        [Fact]
        public async Task GetTopEventsByRevenueAsync_ReturnsTopEvents()
        {
            var testEvents = new List<Event> { new Event { Id = "411eca5c-9be4-4a84-a2be-1119167e71899", Name = "Test Event" } };
            var testDtos = new List<EventSalesDto> { new EventSalesDto { EventId = "411eca5c-9be4-4a84-a2be-1119167e71899", EventName = "Test Event" } };

            _mockRepository.Setup(r => r.GetTopEventsByRevenueAsync(It.IsAny<int>()))
                .ReturnsAsync(testEvents);

            _mockMapper.Setup(m => m.Map<IEnumerable<EventSalesDto>>(testEvents))
                .Returns(testDtos);

            var result = await _service.GetTopEventsByRevenueAsync(5);

            Assert.NotNull(result);
            _mockRepository.Verify(r => r.GetTopEventsByRevenueAsync(5), Times.Once);
        }
    }
}
