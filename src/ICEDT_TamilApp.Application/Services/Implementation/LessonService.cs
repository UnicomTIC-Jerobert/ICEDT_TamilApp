using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ICEDT_TamilApp.Application.DTOs.Request;
using ICEDT_TamilApp.Application.DTOs.Response;
using ICEDT_TamilApp.Application.Exceptions;
using ICEDT_TamilApp.Application.Services.Interfaces;
using ICEDT_TamilApp.Domain.Entities;
using ICEDT_TamilApp.Domain.Interfaces;

namespace ICEDT_TamilApp.Application.Services.Implementation
{
    public class LessonService : ILessonService
    {
        // The service now has a single dependency on IUnitOfWork
        private readonly IUnitOfWork _unitOfWork;

        public LessonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> UpdateLessonAsync(int lessonId, LessonRequestDto updateDto)
        {
            var lessonToUpdate = await _unitOfWork.Lessons.GetByIdAsync(lessonId);
            if (lessonToUpdate == null)
            {
                throw new NotFoundException($"Lesson with ID {lessonId} not found.");
            }

            var otherLessonsInLevel = (
                await _unitOfWork.Lessons.GetAllLessonsByLevelIdAsync(lessonToUpdate.LevelId)
            ).Where(l => l.LessonId != lessonId);

            if (otherLessonsInLevel.Any(l => string.Equals(l.LessonName, updateDto.LessonName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ConflictException($"Another lesson with the name '{updateDto.LessonName}' already exists in this level.");
            }
            if (otherLessonsInLevel.Any(l => l.SequenceOrder == updateDto.SequenceOrder))
            {
                throw new ConflictException($"Another lesson with sequence order {updateDto.SequenceOrder} already exists in this level.");
            }

            lessonToUpdate.LessonName = updateDto.LessonName;
            lessonToUpdate.Description = updateDto.Description;
            lessonToUpdate.SequenceOrder = updateDto.SequenceOrder;

            await _unitOfWork.Lessons.UpdateAsync(lessonToUpdate);
            
            // Commit the transaction
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<LessonResponseDto> CreateLessonToLevelAsync(int levelId, LessonRequestDto dto)
        {
            var level = await _unitOfWork.Levels.GetByIdAsync(levelId);
            if (level == null)
            {
                throw new NotFoundException($"Level with ID {levelId} not found.");
            }

            var existingLessons = await _unitOfWork.Lessons.GetAllLessonsByLevelIdAsync(levelId);
            if (existingLessons.Any(l => string.Equals(l.LessonName, dto.LessonName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ConflictException($"A lesson with the name '{dto.LessonName}' already exists in this level.");
            }
            if (existingLessons.Any(l => l.SequenceOrder == dto.SequenceOrder))
            {
                throw new ConflictException($"A lesson with Sequence Order {dto.SequenceOrder} already exists in this level.");
            }

            var lesson = new Lesson
            {
                LevelId = levelId,
                LessonName = dto.LessonName,
                Description = dto.Description,
                SequenceOrder = dto.SequenceOrder,
            };

            var newLesson = await _unitOfWork.Lessons.CreateAsync(lesson);
            
            // Commit the transaction
            await _unitOfWork.CompleteAsync();

            return MapToResponseDto(newLesson);
        }

        public async Task<bool> DeleteLessonAsync(int lessonId)
        {
            var lesson = await _unitOfWork.Lessons.GetByIdAsync(lessonId);
            if (lesson == null)
            {
                throw new NotFoundException($"Lesson with ID {lessonId} not found for deletion.");
            }

            await _unitOfWork.Lessons.DeleteAsync(lessonId);
            
            // Commit the transaction
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<List<LessonResponseDto>> GetLessonsByLevelIdAsync(int levelId)
        {
            var levelExists = await _unitOfWork.Levels.LevelExistsAsync(levelId); // Assuming LevelExistsAsync is in ILevelRepository
            if (!levelExists)
            {
                throw new NotFoundException($"Level with ID {levelId} not found.");
            }
            
            var lessonsForLevel = await _unitOfWork.Lessons.GetAllLessonsByLevelIdAsync(levelId);
            return lessonsForLevel.Select(MapToResponseDto).ToList();
        }

        public async Task<LessonResponseDto?> GetLessonByIdAsync(int lessonId)
        {
            var lesson = await _unitOfWork.Lessons.GetByIdAsync(lessonId);
            if (lesson == null)
            {
                throw new NotFoundException($"Lesson with ID {lessonId} not found.");
            }
            return MapToResponseDto(lesson);
        }

        // It seems RemoveLessonFromLevelAsync was just a wrapper for DeleteLessonAsync,
        // so its implementation remains simple.
        public async Task RemoveLessonFromLevelAsync(int levelId, int lessonId)
        {
            // The logic inside ensures the lesson exists before attempting to delete.
            await DeleteLessonAsync(lessonId);
        }

        // A private helper for mapping to avoid code duplication
        private LessonResponseDto MapToResponseDto(Lesson lesson)
        {
            return new LessonResponseDto
            {
                LessonId = lesson.LessonId,
                LevelId = lesson.LevelId,
                LessonName = lesson.LessonName,
                Description = lesson.Description,
                SequenceOrder = lesson.SequenceOrder,
            };
        }
    }
}