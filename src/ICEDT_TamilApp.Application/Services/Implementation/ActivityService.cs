using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ICEDT_TamilApp.Application.DTOs.Request;
using ICEDT_TamilApp.Application.DTOs.Response;
using ICEDT_TamilApp.Application.Exceptions;
using ICEDT_TamilApp.Application.Services.Interfaces;
using ICEDT_TamilApp.Domain.Entities;
using ICEDT_TamilApp.Domain.Interfaces;

namespace ICEDT_TamilApp.Application.Services.Implementation
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepo;
        private readonly IActivityTypeRepository _typeRepo;
        private readonly IMainActivityRepository _mainActivityRepo;

        public ActivityService(
            IActivityRepository activityRepo,
            IActivityTypeRepository typeRepo,
            IMainActivityRepository mainActivityRepo
        )
        {
            _activityRepo = activityRepo;
            _typeRepo = typeRepo;
            _mainActivityRepo = mainActivityRepo;
        }

        public async Task<ActivityResponseDto> GetActivityAsync(int id)
        {
            var activity = await _activityRepo.GetByIdAsync(id);
            if (activity == null)
            {
                throw new NotFoundException($"Activity with ID {id} not found.");
            }
            return MapToActivityResponseDto(activity);
        }

        public async Task<List<ActivityResponseDto>> GetAllActivitiesAsync()
        {
            var activities = await _activityRepo.GetAllAsync();
            return activities.Select(MapToActivityResponseDto).ToList();
        }

        public async Task<List<ActivityResponseDto>> GetActivitiesByLessonIdAsync(int lessonId)
        {
            var lessonExists = await _activityRepo.LessonExistsAsync(lessonId);
            if (!lessonExists)
            {
                throw new NotFoundException($"Lesson with ID {lessonId} not found.");
            }
            var activities = await _activityRepo.GetByLessonIdAsync(lessonId);
            return activities.Select(MapToActivityResponseDto).ToList();
        }

        public async Task<ActivityResponseDto> CreateActivityAsync(ActivityRequestDto dto)
        {
            var validationTasks = new List<Task<bool>>
            {
                _activityRepo.LessonExistsAsync(dto.LessonId),
                _typeRepo.ActivityTypeExistsAsync(dto.ActivityTypeId),
                _mainActivityRepo.MainActivityTypeExistsAsync(dto.MainActivityId),
            };

            await Task.WhenAll(validationTasks);

            if (!validationTasks[0].Result)
                throw new NotFoundException(
                    $"Cannot create activity: Lesson with ID {dto.LessonId} not found."
                );

            if (!validationTasks[1].Result)
                throw new NotFoundException(
                    $"Cannot create activity: Activity Type with ID {dto.ActivityTypeId} not found."
                );

            if (!validationTasks[2].Result)
                throw new NotFoundException(
                    $"Cannot create activity: Main Activity with ID {dto.MainActivityId} not found."
                );

            var existingActivitiesInLesson = await _activityRepo.GetByLessonIdAsync(dto.LessonId);
            if (existingActivitiesInLesson.Any(a => a.SequenceOrder == dto.SequenceOrder))
            {
                throw new ConflictException(
                    $"Sequence order {dto.SequenceOrder} is already in use for this lesson."
                );
            }

            var activity = new Activity
            {
                LessonId = dto.LessonId,
                ActivityTypeId = dto.ActivityTypeId,
                MainActivityId = dto.MainActivityId,
                Title = dto.Title,
                SequenceOrder = dto.SequenceOrder,
                ContentJson = dto.ContentJson,
            };

            await _activityRepo.CreateAsync(activity);
            return MapToActivityResponseDto(activity);
        }

        public async Task UpdateActivityAsync(int id, ActivityRequestDto dto)
        {
            var activity = await _activityRepo.GetByIdAsync(id);
            if (activity == null)
            {
                throw new NotFoundException($"Activity with ID {id} not found.");
            }

            var mainActivityExists = await _mainActivityRepo.MainActivityTypeExistsAsync(
                dto.MainActivityId
            );
            if (!mainActivityExists)
                throw new NotFoundException(
                    $"Cannot update activity: Main Activity with ID {dto.MainActivityId} not found."
                );

            var activityTypeExists = await _typeRepo.ActivityTypeExistsAsync(dto.ActivityTypeId);
            if (!activityTypeExists)
                throw new NotFoundException(
                    $"Cannot update activity: Activity Type with ID {dto.ActivityTypeId} not found."
                );

            activity.LessonId = dto.LessonId;
            activity.ActivityTypeId = dto.ActivityTypeId;
            activity.MainActivityId = dto.MainActivityId;
            activity.Title = dto.Title;
            activity.SequenceOrder = dto.SequenceOrder;
            activity.ContentJson = dto.ContentJson;

            await _activityRepo.UpdateAsync(activity);
        }

        public async Task DeleteActivityAsync(int id)
        {
            var activity = await _activityRepo.GetByIdAsync(id);
            if (activity == null)
            {
                throw new NotFoundException($"Activity with ID {id} not found.");
            }
            await _activityRepo.DeleteAsync(id);
        }

        private ActivityResponseDto MapToActivityResponseDto(Activity activity)
        {
            return new ActivityResponseDto
            {
                ActivityId = activity.ActivityId,
                LessonId = activity.LessonId,
                ActivityTypeId = activity.ActivityTypeId,
                MainActivityTypeId = activity.MainActivityId,
                Title = activity.Title,
                SequenceOrder = activity.SequenceOrder,
                ContentJson = activity.ContentJson,
            };
        }
    }
}
