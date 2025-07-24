using ICEDT_TamilApp.Application.DTOs.Request;
using ICEDT_TamilApp.Application.DTOs.Response;
using ICEDT_TamilApp.Application.Exceptions;
using ICEDT_TamilApp.Application.Services.Interfaces;
using ICEDT_TamilApp.Domain.Entities;
using ICEDT_TamilApp.Domain.Interfaces;

namespace ICEDT_TamilApp.Application.Services.Implementation
{
    public class LevelService : ILevelService
    {
        private readonly ILevelRepository _repo;

        public LevelService(ILevelRepository repo) => _repo = repo;

        public async Task<LevelResponseDto> GetLevelAsync(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid Level ID.");
            var level = await _repo.GetByIdAsync(id);
            if (level == null)
                throw new NotFoundException("Level not found.");
            return MapToResponseDto(level);
        }

        public async Task<List<LevelResponseDto>> GetAllLevelsAsync()
        {
            var levels = await _repo.GetAllAsync();
            return levels.Select(MapToResponseDto).ToList();
        }

        public async Task<LevelResponseDto> CreateLevelAsync(LevelRequestDto dto)
        {
            if (await _repo.SequenceOrderExistsAsync(dto.SequenceOrder))
                throw new BadRequestException(
                    $"Sequence order {dto.SequenceOrder} is already in use."
                );

            /*
            var existingLevels = await _repo.GetAllAsync();
            if (existingLevels.Any(l => l.SequenceOrder == dto.SequenceOrder))
            {
                foreach (var level in existingLevels.Where(l => l.SequenceOrder >= dto.SequenceOrder))
                {
                    level.SequenceOrder++;
                    await _repo.UpdateAsync(level);
                }
            }
            */

            var level = new Level { LevelName = dto.LevelName, SequenceOrder = dto.SequenceOrder };
            await _repo.CreateAsync(level);
            return MapToResponseDto(level);
        }

        public async Task UpdateLevelAsync(int id, LevelRequestDto dto)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid Level ID.");
            var level = await _repo.GetByIdAsync(id);
            if (level == null)
                throw new NotFoundException("Level not found.");

            if (level.SequenceOrder != dto.SequenceOrder)
            {
                if (await _repo.SequenceOrderExistsAsync(dto.SequenceOrder))
                    throw new BadRequestException(
                        $"Sequence order {dto.SequenceOrder} is already in use."
                    );
            }

            /*
            if (level.SequenceOrder != dto.SequenceOrder)
            {
                var existingLevels = await _repo.GetAllAsync();
                if (existingLevels.Any(l => l.SequenceOrder == dto.SequenceOrder && l.LevelId != id))
                {
                    foreach (var existingLevel in existingLevels.Where(l => l.SequenceOrder >= dto.SequenceOrder && l.LevelId != id))
                    {
                        existingLevel.SequenceOrder++;
                        await _repo.UpdateAsync(existingLevel);
                    }
                }
            }
            */

            level.LevelName = dto.LevelName;
            level.SequenceOrder = dto.SequenceOrder;
            await _repo.UpdateAsync(level);
        }

        public async Task DeleteLevelAsync(int id)
        {
            var level = await _repo.GetByIdAsync(id);
            if (level == null)
                throw new NotFoundException("Level not found.");
            await _repo.DeleteAsync(id);
        }

        private LevelResponseDto MapToResponseDto(Level level)
        {
            return new LevelResponseDto
            {
                LevelId = level.LevelId,
                LevelName = level.LevelName,
                SequenceOrder = level.SequenceOrder,
            };
        }
    }
}
