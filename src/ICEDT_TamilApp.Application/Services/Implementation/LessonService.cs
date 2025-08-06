using System;
using System.Linq;
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
        private readonly ILessonRepository _lessonRepo;
        private readonly ILevelRepository _levelRepo;

        public LessonService(ILessonRepository lessonRepo, ILevelRepository levelRepo)
        {
            _lessonRepo = lessonRepo;
            _levelRepo = levelRepo;
        }

        public async Task<bool> UpdateLessonAsync(int lessonId, LessonRequestDto updateDto)
        {
            var lessonToUpdate = await _lessonRepo.GetByIdAsync(lessonId);
            if (lessonToUpdate == null)
            {
                throw new NotFoundException($"Lesson with ID {lessonId} not found.");
            }

            var otherLessonsInLevel = (
                await _lessonRepo.GetAllLessonsByLevelIdAsync(lessonToUpdate.LevelId)
            ).Where(l => l.LessonId != lessonId);

            if (
                otherLessonsInLevel.Any(l =>
                    string.Equals(
                        l.LessonName,
                        updateDto.LessonName,
                        StringComparison.OrdinalIgnoreCase
                    )
                )
            )
            {
                throw new ConflictException(
                    $"Another lesson with the name '{updateDto.LessonName}' already exists in this level."
                );
            }
            if (otherLessonsInLevel.Any(l => l.SequenceOrder == updateDto.SequenceOrder))
            {
                throw new ConflictException(
                    $"Another lesson with sequence order {updateDto.SequenceOrder} already exists in this level."
                );
            }

            lessonToUpdate.LessonName = updateDto.LessonName;
            lessonToUpdate.Description = updateDto.Description;
            lessonToUpdate.SequenceOrder = updateDto.SequenceOrder;

            return await _lessonRepo.UpdateAsync(lessonToUpdate);
        }

        public async Task<LessonResponseDto> CreateLessonToLevelAsync(
            int levelId,
            LessonRequestDto dto
        )
        {
            var level = await _levelRepo.GetByIdAsync(levelId);
            if (level == null)
            {
                throw new NotFoundException($"Level with ID {levelId} not found.");
            }

            var existingLessons = await _lessonRepo.GetAllLessonsByLevelIdAsync(levelId);
            if (
                existingLessons.Any(l =>
                    string.Equals(l.LessonName, dto.LessonName, StringComparison.OrdinalIgnoreCase)
                )
            )
            {
                throw new ConflictException(
                    $"A lesson with the name '{dto.LessonName}' already exists in this level."
                );
            }
            if (existingLessons.Any(l => l.SequenceOrder == dto.SequenceOrder))
            {
                throw new ConflictException(
                    $"A lesson with Sequence Order {dto.SequenceOrder} already exists in this level."
                );
            }

            var lesson = new Lesson
            {
                LevelId = levelId,
                LessonName = dto.LessonName,
                Description = dto.Description,
                SequenceOrder = dto.SequenceOrder,
            };

            var newLesson = await _lessonRepo.CreateAsync(lesson);

            return new LessonResponseDto
            {
                LessonId = newLesson.LessonId,
                LevelId = newLesson.LevelId,
                LessonName = newLesson.LessonName,
                Description = newLesson.Description,
                SequenceOrder = newLesson.SequenceOrder,
            };
        }

        public async Task<bool> DeleteLessonAsync(int lessonId)
        {
            var success = await _lessonRepo.DeleteAsync(lessonId);
            if (!success)
            {
                throw new NotFoundException($"Lesson with ID {lessonId} not found for deletion.");
            }
            return true;
        }

        public async Task<List<LessonResponseDto>> GetLessonsByLevelIdAsync(int levelId)
        {
            var level = await _levelRepo.GetByIdAsync(levelId);
            if (level == null)
            {
                throw new NotFoundException("Level not found.");
            }
            var lessonsForLevel = await _lessonRepo.GetAllLessonsByLevelIdAsync(levelId);

            var lessons = lessonsForLevel
                     .Select(l => new LessonResponseDto
                     {
                         LessonId = l.LessonId,
                         LevelId = l.LevelId,
                         LessonName = l.LessonName,
                         Description = l.Description,
                         SequenceOrder = l.SequenceOrder,
                     })
                     .ToList();

            return lessons;

        }

        public async Task RemoveLessonFromLevelAsync(int levelId, int lessonId)
        {
            await DeleteLessonAsync(lessonId);
        }

        public async Task<LessonResponseDto?> GetLessonByIdAsync(int lessonId)
        {
            var lesson = await _lessonRepo.GetByIdAsync(lessonId);
             
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
