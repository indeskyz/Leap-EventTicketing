using AutoMapper;
using EventTicketing.Cache.Services;
using EventTicketing.Data.Entities.Events;
using EventTicketing.Data.Repositories.Events;
using EventTicketing.DTOs.Events;
using EventTicketing.DTOs.Pagination;
using EventTicketing.Services.Events;
using Moq;

namespace EventTicketing.Tests.Events
{
    public class EventServiceTests
    {
        private readonly Mock<IEventRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ICacheService> _mockCacheService;
        private readonly IEventService _service;

        public EventServiceTests()
        {
            _mockRepository = new Mock<IEventRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockCacheService = new Mock<ICacheService>();
            _service = new EventService(_mockRepository.Object, _mockMapper.Object, _mockCacheService.Object);
        }

        [Fact]
        public async Task GetUpcomingEventsAsync_ValidDays_ReturnsEvents()
        {
            var request = new PaginationRequest { PageNumber = 1, PageSize = 10 };
            var testEvents = new List<Event> { new() { Id = "411eca5c-9be4-4a84-a2be-666167e71899", Name = "Test Event" } };
            var testDtos = new List<EventDto> { new EventDto { Id = "411eca5c-9be4-4a84-a2be-666167e71899", Name = "Test Event" } };

            _mockRepository
                .Setup(r => r.GetUpcomingEventsAsync(It.IsAny<DateTime>(), request.PageNumber, request.PageSize))
                .ReturnsAsync((testEvents, 1));

            _mockMapper
                .Setup(m => m.Map<IEnumerable<EventDto>>(testEvents))
                .Returns(testDtos);

            var result = await _service.GetUpcomingEventsAsync(30, request);

            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal(1, result.TotalCount);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(10, result.PageSize);

            _mockRepository.Verify(r => r.GetUpcomingEventsAsync(It.IsAny<DateTime>(), 1, 10), Times.Once);
        }

        [Fact]
        public async Task GetUpcomingEventsAsync_InvalidDays_ThrowsArgumentException()
        {
            var request = new PaginationRequest { PageNumber = 1, PageSize = 10 };

            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.GetUpcomingEventsAsync(0, request)); // 0 is invalid
            Assert.Equal("Days must be a positive number (Parameter 'days')", ex.Message);
        }


    }
}
