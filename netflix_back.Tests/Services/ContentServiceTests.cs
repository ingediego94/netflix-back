using Moq;
using FluentAssertions;
using AutoMapper;
using netflix_back.Application.Services;
using netflix_back.Application.Interfaces;
using netflix_back.Domain.Entities;
using netflix_back.Domain.Interfaces;

namespace netflix_back.Tests.Services;

public class ContentServiceTests
{
    private readonly Mock<IGeneralRepository<Content>> _contentRepoMock;
    private readonly Mock<IVideoRepository> _videoRepoMock;
    private readonly Mock<ICloudinaryService> _cloudinaryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ContentService _contentService;

    public ContentServiceTests()
    {
        _contentRepoMock = new Mock<IGeneralRepository<Content>>();
        _videoRepoMock = new Mock<IVideoRepository>();
        _cloudinaryMock = new Mock<ICloudinaryService>();
        _mapperMock = new Mock<IMapper>();

        _contentService = new ContentService(
            _contentRepoMock.Object, 
            _videoRepoMock.Object, 
            _cloudinaryMock.Object, 
            _mapperMock.Object);
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallCloudinaryDelete_WhenContentHasVideo()
    {
        // Arrange
        var contentId = 1;
        var videoId = 99;
        var video = new Video 
        { 
            Id = videoId, 
            PublicIdVideo = "vid_123", 
            PublicIdPicture = "img_123" 
        };
        var content = new Content { Id = contentId, VideoId = videoId, Video = video };

        // 1. Setup para encontrar los datos
        _contentRepoMock.Setup(r => r.GetByIdAsync(contentId)).ReturnsAsync(content);
        _videoRepoMock.Setup(r => r.GetByIdAsync(videoId)).ReturnsAsync(video);

        // 2. IMPORTANTE: Setup para que el borrado devuelva TRUE
        // Sin esto, el Mock devuelve false por defecto y el test falla.
        _contentRepoMock.Setup(r => r.DeleteAsync(content)).ReturnsAsync(true);
        _videoRepoMock.Setup(r => r.RemoveAsync(videoId)).ReturnsAsync(true);

        // Act
        var result = await _contentService.DeleteAsync(contentId);

        // Assert
        result.Should().BeTrue(); // Ahora sí será True
        _cloudinaryMock.Verify(c => c.DeleteFileAsync("vid_123", "video"), Times.Once);
        _cloudinaryMock.Verify(c => c.DeleteFileAsync("img_123", "image"), Times.Once);
        _videoRepoMock.Verify(r => r.RemoveAsync(videoId), Times.Once);
        _contentRepoMock.Verify(r => r.DeleteAsync(content), Times.Once);
    }
}