using netflix_back.Application.DTOs;

namespace netflix_back.Application.Interfaces;

public interface IContentService
{
    Task<IEnumerable<ContentResponseDto>> GetAllAsync();
    Task<ContentResponseDto?> GetByIdAsync(int id);
    Task<ContentResponseDto> CreateAsync(ContentCreateDto dto);
    Task<ContentResponseDto?> UpdateAsync(int id, ContentUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}