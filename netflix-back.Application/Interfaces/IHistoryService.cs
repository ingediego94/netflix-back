using netflix_back.Application.DTOs;

namespace netflix_back.Application.Interfaces;

public interface IHistoryService
{
    Task<IEnumerable<HistoryResponseDto>> GetAllAsync();
    Task<HistoryResponseDto?> GetByIdAsync(int id);
    Task<HistoryResponseDto> CreateOrUpdateAsync(HistoryCreateDto dto);
    // Task<HistoryResponseDto?> UpdateAsync(int id, HistoryUpdateDto dto);
}