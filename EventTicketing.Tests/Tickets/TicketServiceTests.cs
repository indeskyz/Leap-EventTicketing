using AutoMapper;
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
        private readonly Mock<ITicketRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ITicketService _service;

        public TicketServiceTests()
        {
            _mockRepository = new Mock<ITicketRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new TicketService(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetTopEventsByRevenueAsync_ReturnsTopEvents()
        {
            // Arrange
            var testEvents = new List<Event> { new Event { Id = 1, Name = "Test Event" } };
            var testDtos = new List<EventSalesDto> { new EventSalesDto { EventId = 1, EventName = "Test Event" } };

            _mockRepository.Setup(r => r.GetTopEventsByRevenueAsync(It.IsAny<int>()))
                .ReturnsAsync(testEvents);

            _mockMapper.Setup(m => m.Map<IEnumerable<EventSalesDto>>(testEvents))
                .Returns(testDtos);

            // Act
            var result = await _service.GetTopEventsByRevenueAsync(5);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            _mockRepository.Verify(r => r.GetTopEventsByRevenueAsync(5), Times.Once);
        }
    }
}
