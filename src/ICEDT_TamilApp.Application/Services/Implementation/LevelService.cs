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
        // The service now depends on the Unit of Work, not an individual repository.
        private readonly IUnitOfWork _unitOfWork;

        public LevelService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<LevelResponseDto> GetLevelAsync(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid Level ID.");
            
            // Access the specific repository through the Unit of Work property.
            var level = await _unitOfWork.Levels.GetByIdAsync(id);
            
            if (level == null)
                throw new NotFoundException("Level not found.");
            
            return MapToResponseDto(level);
        }

        public async Task<List<LevelResponseDto>> GetAllLevelsAsync()
        {
            var levels = await _unitOfWork.Levels.GetAllAsync();
            return levels.Select(MapToResponseDto).ToList();
        }

        public async Task<LevelResponseDto> CreateLevelAsync(LevelRequestDto dto)
        {
            // Perform validation using the repository.
            if (await _unitOfWork.Levels.SequenceOrderExistsAsync(dto.SequenceOrder))
                throw new BadRequestException($"Sequence order {dto.SequenceOrder} is already in use.");

            if (await _unitOfWork.Levels.SlugExistsAsync(dto.Slug))
                 throw new BadRequestException($"Slug '{dto.Slug}' is already in use.");

            var level = new Level 
            { 
                LevelName = dto.LevelName, 
                SequenceOrder = dto.SequenceOrder, 
                Slug = dto.Slug 
            };

            // Add the new entity to the context via the repository.
            await _unitOfWork.Levels.CreateAsync(level);
            
            // *** THE KEY CHANGE ***
            // Commit all changes made in this transaction to the database.
            await _unitOfWork.CompleteAsync();

            return MapToResponseDto(level);
        }

        public async Task UpdateLevelAsync(int id, LevelRequestDto dto)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid Level ID.");
            
            var level = await _unitOfWork.Levels.GetByIdAsync(id);
            if (level == null)
                throw new NotFoundException("Level not found.");

            // Check if the sequence order is being changed AND if the new one is already taken.
            if (level.SequenceOrder != dto.SequenceOrder && await _unitOfWork.Levels.SequenceOrderExistsAsync(dto.SequenceOrder))
            {
                throw new BadRequestException($"Sequence order {dto.SequenceOrder} is already in use.");
            }
            
            // Check if the slug is being changed AND if the new one is already taken.
            if (level.Slug != dto.Slug && await _unitOfWork.Levels.SlugExistsAsync(dto.Slug))
            {
                 throw new BadRequestException($"Slug '{dto.Slug}' is already in use.");
            }

            // Update the entity's properties in memory.
            level.LevelName = dto.LevelName;
            level.SequenceOrder = dto.SequenceOrder;
            level.Slug = dto.Slug;

            // The repository's UpdateAsync method just marks the entity as Modified.
            await _unitOfWork.Levels.UpdateAsync(level);

            // *** THE KEY CHANGE ***
            // Commit the update to the database.
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteLevelAsync(int id)
        {
            var level = await _unitOfWork.Levels.GetByIdAsync(id);
            if (level == null)
                throw new NotFoundException("Level not found.");
            
            // The repository's DeleteAsync method removes the entity from the context.
            await _unitOfWork.Levels.DeleteAsync(id);

            // *** THE KEY CHANGE ***
            // Commit the deletion to the database.
            await _unitOfWork.CompleteAsync();
        }

        // The private DTO mapper function remains the same.
        private LevelResponseDto MapToResponseDto(Level level)
        {
            return new LevelResponseDto
            {
                LevelId = level.LevelId,
                LevelName = level.LevelName,
                Slug = level.Slug,
                SequenceOrder = level.SequenceOrder,
            };
        }
    }
}