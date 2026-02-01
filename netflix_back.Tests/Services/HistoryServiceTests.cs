using Moq;
using FluentAssertions;
using AutoMapper;
using netflix_back.Application.Services;
using netflix_back.Application.DTOs;
using netflix_back.Domain.Entities;
using netflix_back.Domain.Interfaces;

namespace netflix_back.Tests.Services;

public class HistoryServiceTests
{
    private readonly Mock<IHistoryRepository> _historyRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly HistoryService _historyService;

    public HistoryServiceTests()
    {
        _historyRepoMock = new Mock<IHistoryRepository>();
        _mapperMock = new Mock<IMapper>();
        _historyService = new HistoryService(_historyRepoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task CreateOrUpdateAsync_ShouldUpdateExisting_WhenRecordAlreadyExists()
    {
        // Arrange
        var dto = new HistoryCreateDto { UserId = 1, VideoId = 10, Progress = 500 };
        var existingHistory = new History { Id = 5, UserId = 1, VideoId = 10, Progress = 100 };

        _historyRepoMock.Setup(r => r.GetByUserAndVideoAsync(dto.UserId, dto.VideoId))
            .ReturnsAsync(existingHistory);
        
        _historyRepoMock.Setup(r => r.UpdateAsync(It.IsAny<History>()))
            .ReturnsAsync(existingHistory);

        // Act
        await _historyService.CreateOrUpdateAsync(dto);

        // Assert
        existingHistory.Progress.Should().Be(500);
        _historyRepoMock.Verify(r => r.UpdateAsync(It.IsAny<History>()), Times.Once);
        _historyRepoMock.Verify(r => r.CreateAsync(It.IsAny<History>()), Times.Never);
    }

    [Fact]
    public async Task CreateOrUpdateAsync_ShouldThrowException_WhenProgressIsNegative()
    {
        // Arrange
        var dto = new HistoryCreateDto { UserId = 1, VideoId = 10, Progress = -1 };

        // Act
        Func<Task> act = async () => await _historyService.CreateOrUpdateAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*no puede ser negativo*");
    }
}