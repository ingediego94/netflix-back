using Moq;
using FluentAssertions;
using netflix_back.Application.Services;
using netflix_back.Domain.Interfaces;
using netflix_back.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using netflix_back.Application.DTOs;

namespace netflix_back.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IGeneralRepository<User>> _userRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _userRepoMock = new Mock<IGeneralRepository<User>>();
        _mapperMock = new Mock<IMapper>();
        _configMock = new Mock<IConfiguration>();
        _authService = new AuthService(_userRepoMock.Object, _mapperMock.Object, _configMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrowException_WhenUserAlreadyExists()
    {
        // Arrange (Preparar)
        var dto = new RegisterDto { Email = "diego@gmail.com", Password = "123" };
        var existingUsers = new List<User> { new User { Email = "diego@gmail.com" } };
        
        _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(existingUsers);

        // Act (Actuar)
        Func<Task> act = async () => await _authService.RegisterAsync(dto);

        // Assert (Verificar)
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("*ya se encuentra registrado*");
    }
}