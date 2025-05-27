using AutoMapper;
using EventTicketing.Cache.Services;
using EventTicketing.Data.Entities.Events;
using EventTicketing.Data.Repositories.Events;
using EventTicketing.DTOs.Events;
using EventTicketing.Services.Events;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTicketing.Tests.Cache
{
    public class CacheEventsTests
    {
        private readonly Mock<IEventRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ICacheService> _mockCacheService;
        private readonly IEventService _service;
        public CacheEventsTests()
        {
            _mockRepository = new Mock<IEventRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockCacheService = new Mock<ICacheService>();
            _service = new EventService(_mockRepository.Object, _mockMapper.Object, _mockCacheService.Object);
        }
        [Fact]
        public async Task CachedGetByIdAsync_ReturnsCachedEvent_WhenExistsInCache()
        {
            var eventId = "411eca5c-9be4-4a84-a2be-666167e71899";
            var cachedEventDto = new EventDto { Id = eventId, Name = "Cached Event" };

            _mockCacheService
                .Setup(c => c.GetOrSetAsync(
                    $"events:byid:{eventId}",
                    It.IsAny<Func<Task<EventDto>>>(),
                    It.Is<CacheOptions>(o =>
                        o.Expiration == TimeSpan.FromHours(2) &&
                        !o.BypassLocalCache)))
                .ReturnsAsync(cachedEventDto);

            var result = await _service.CachedGetByIdAsync(eventId);

            Assert.Equal(cachedEventDto, result);
            _mockRepository.Verify(r => r.GetByIdAsync(eventId), Times.Never);
        }

        [Fact]
        public async Task CachedGetByIdAsync_FetchesFromRepository_WhenCacheMiss()
        {
            var eventId = "411eca5c-9be4-4a84-a2be-666167e71899";
            var dbEvent = new Event { Id = eventId, Name = "DB Event" };
            var mappedDto = new EventDto { Id = eventId, Name = "DB Event" };

            _mockCacheService
                .Setup(c => c.GetOrSetAsync(
                    $"events:byid:{eventId}",
                    It.IsAny<Func<Task<EventDto>>>(),
                    It.IsAny<CacheOptions>()))
                .Returns((string key, Func<Task<EventDto>> factory, CacheOptions _) => factory());

            _mockRepository
                .Setup(r => r.GetByIdAsync(eventId))
                .ReturnsAsync(dbEvent);

            _mockMapper
                .Setup(m => m.Map<EventDto>(dbEvent))
                .Returns(mappedDto);

            var result = await _service.CachedGetByIdAsync(eventId);

            Assert.Equal(mappedDto, result);
            _mockRepository.Verify(r => r.GetByIdAsync(eventId), Times.Once);
        }

        [Fact]
        public async Task CachedGetByIdAsync_ThrowsKeyNotFoundException_WhenEventNotFound()
        {
            var eventId = "non-existent-id";

            _mockCacheService
                .Setup(c => c.GetOrSetAsync(
                    $"events:byid:{eventId}",
                    It.IsAny<Func<Task<EventDto>>>(),
                    It.IsAny<CacheOptions>()))
                .Returns((string key, Func<Task<EventDto>> factory, CacheOptions _) => factory());

            _mockRepository
                .Setup(r => r.GetByIdAsync(eventId))
                .ReturnsAsync(value: null!);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.CachedGetByIdAsync(eventId));

            Assert.Equal($"Event with ID {eventId} not found.", ex.Message);
        }
    }
}

