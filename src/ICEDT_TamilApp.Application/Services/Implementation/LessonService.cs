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
            this._lessonRepo = lessonRepo;
            this._levelRepo = levelRepo;
        }

        public async Task<LessonResponseDto> CreateLessonToLevelAsync(
            int levelId,
            LessonRequestDto dto
        )
        {
            var level = await _levelRepo.GetByIdAsync(levelId);
            if (level == null)
                throw new NotFoundException("Level not found.");

            if (level.Lessons?.Any(l => l.LessonName == dto.LessonName) == true)
                throw new ConflictException(
                    "A lesson with the same name already exists in this level."
                );
            if (level.Lessons?.Any(l => l.SequenceOrder == dto.SequenceOrder) == true)
                throw new ConflictException("A lesson with the same SequenceOrder already exists.");

            var lesson = new Lesson
            {
                LevelId = levelId,
                LessonName = dto.LessonName,
                Description = dto.Description,
                SequenceOrder = dto.SequenceOrder,
            };

            level.Lessons ??= new List<Lesson>();
            level.Lessons.Add(lesson);
            await _levelRepo.UpdateAsync(level);

            return new LessonResponseDto
            {
                LessonId = lesson.LessonId,
                LevelId = lesson.LevelId,
                LessonName = lesson.LessonName,
                Description = lesson.Description,
                SequenceOrder = lesson.SequenceOrder,
            };
        }

        public async Task RemoveLessonFromLevelAsync(int levelId, int lessonId)
        {
            var level = await this._levelRepo.GetByIdAsync(levelId);
            if (level == null)
                throw new NotFoundException("Level not found.");
            var lesson = level.Lessons?.FirstOrDefault(l => l.LessonId == lessonId);
            if (lesson == null)
                throw new NotFoundException("Lesson not found in this level.");
            level.Lessons.Remove(lesson);
            await this._levelRepo.UpdateAsync(level);
        }

        public async Task<LevelWithLessonsResponseDto> GetLevelWithLessonsAsync(int levelId)
        {
            var level = await this._levelRepo.GetByIdAsync(levelId);
            if (level == null)
                throw new NotFoundException("Level not found.");
            return new LevelWithLessonsResponseDto
            {
                LevelId = level.LevelId,
                LevelName = level.LevelName,
                SequenceOrder = level.SequenceOrder,
                Lessons =
                    level
                        .Lessons?.Select(l => new LessonResponseDto
                        {
                            LessonId = l.LessonId,
                            LevelId = l.LevelId,
                            LessonName = l.LessonName,
                            Description = l.Description,
                            SequenceOrder = l.SequenceOrder,
                        })
                        .ToList() ?? new List<LessonResponseDto>(),
            };
        }

        public Task<LessonRequestDto?> GetLessonByIdAsync(int lessonId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateLessonAsync(int lessonId, LessonRequestDto updateDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteLessonAsync(int lessonId)
        {
            throw new NotImplementedException();
        }

        Task<LessonResponseDto?> ILessonService.GetLessonByIdAsync(int lessonId)
        {
            throw new NotImplementedException();
        }
    }
}
