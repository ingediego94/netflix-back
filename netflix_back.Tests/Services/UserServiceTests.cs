using Moq;
using FluentAssertions;
using AutoMapper;
using netflix_back.Application.Services;
using netflix_back.Application.DTOs;
using netflix_back.Domain.Entities;
using netflix_back.Domain.Interfaces;

namespace netflix_back.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IGeneralRepository<User>> _userRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        // 1. Inicializamos los Mocks
        _userRepoMock = new Mock<IGeneralRepository<User>>();
        _mapperMock = new Mock<IMapper>();

        // 2. Inyectamos los mocks en el servicio
        _userService = new UserService(_userRepoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task UpdateAsync_ShouldNotUpdatePassword_WhenPasswordInDtoIsEmpty()
    {
        // Arrange (Preparar datos)
        var userId = 1;
        var existingUser = new User 
        { 
            Id = userId, 
            Name = "Diego", 
            PasswordHash = "HashAntiguo" 
        };
        var updateDto = new UserUpdateDto { Name = "Diego Editado", Password = "" };

        // Configuramos el Mock para que devuelva al usuario cuando se busque por ID
        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(existingUser);
        
        // Configuramos el Mock para que devuelva el usuario actualizado al guardar
        _userRepoMock.Setup(r => r.UpdateAsync(It.IsAny<User>())).ReturnsAsync(existingUser);

        // Act (Ejecutar la acción)
        var result = await _userService.UpdateAsync(userId, updateDto);

        // Assert (Verificar resultados)
        existingUser.PasswordHash.Should().Be("HashAntiguo"); // No debió cambiar
        _userRepoMock.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenUserDoesNotExist()
    {
        // Arrange
        int nonExistingId = 99;
        _userRepoMock.Setup(r => r.GetByIdAsync(nonExistingId)).ReturnsAsync((User)null!);

        // Act
        var result = await _userService.DeleteAsync(nonExistingId);

        // Assert
        result.Should().BeFalse();
        _userRepoMock.Verify(r => r.DeleteAsync(It.IsAny<User>()), Times.Never);
    }
}