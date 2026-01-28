using AutoMapper;
using netflix_back.Application.DTOs;
using netflix_back.Application.Interfaces;
using netflix_back.Domain.Entities;
using netflix_back.Domain.Interfaces;

namespace netflix_back.Application.Services;

public class HistoryService : IHistoryService
{
    private readonly IHistoryRepository _historyRepository;
    private readonly IMapper _mapper;

    public HistoryService(IHistoryRepository historyRepository,
        IMapper mapper)
    {
        _historyRepository = historyRepository;
        _mapper = mapper;
    }
    
    // -----------------------------------------------------------
    
    // Get All:
    public async Task<IEnumerable<HistoryResponseDto>> GetAllAsync()
    {
        var results = await _historyRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<HistoryResponseDto>>(results);
    }

    
    // Get By Id:
    public async Task<HistoryResponseDto?> GetByIdAsync(int id)
    {
        var result = await _historyRepository.GetByIdAsync(id);
        return _mapper.Map<HistoryResponseDto>(result);
    }

    
    // Create:
    public async Task<HistoryResponseDto> CreateOrUpdateAsync(HistoryCreateDto dto)
    {
        // 1. Verificar si ya existe el registro
        var existingHistory = await _historyRepository.GetByUserAndVideoAsync(dto.UserId, dto.VideoId);

        if (existingHistory != null)
        {
            // 2. Si existe, actualizamos el progreso y la fecha
            existingHistory.Progress = dto.Progress;
            existingHistory.UpdatedAt = DateTime.UtcNow; // MODIFICADO: Registro de actualización

            var updated = await _historyRepository.UpdateAsync(existingHistory);
            return _mapper.Map<HistoryResponseDto>(updated);
        }

        // 3. Si no existe, creamos uno nuevo
        var newHistory = _mapper.Map<History>(dto);
        newHistory.CreatedAt = DateTime.UtcNow;
        newHistory.UpdatedAt = DateTime.UtcNow;

        var result = await _historyRepository.CreateAsync(newHistory);
        return _mapper.Map<HistoryResponseDto>(result);
    }

    // Nota: UpdateAsync individual puede quedar como respaldo o eliminarse si usas el de arriba
    // public async Task<HistoryResponseDto?> UpdateAsync(int id, HistoryUpdateDto dto)
    // {
    //     var history = await _historyRepository.GetByIdAsync(id);
    //     if (history == null) return null;
    //
    //     _mapper.Map(dto, history);
    //     history.UpdatedAt = DateTime.UtcNow; // MODIFICADO: Actualizar timestamp
    //
    //     var result = await _historyRepository.UpdateAsync(history);
    //     return _mapper.Map<HistoryResponseDto>(result);
    // }
}