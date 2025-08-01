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
using Microsoft.Extensions.Logging;

namespace ICEDT_TamilApp.Application.Services.Implementation
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepo;
        private readonly IActivityTypeRepository _typeRepo;
        private readonly IMainActivityRepository _mainActivityRepo;
        private readonly ILessonRepository _lessonRepo;
        private readonly ILogger<ActivityService> _logger;

        public ActivityService(
            IActivityRepository activityRepo,
            IActivityTypeRepository typeRepo,
            IMainActivityRepository mainActivityRepo,
            ILessonRepository lessonRepo,
            ILogger<ActivityService> logger
        )
        {
            _activityRepo = activityRepo;
            _typeRepo = typeRepo;
            _mainActivityRepo = mainActivityRepo;
            _lessonRepo = lessonRepo;
            _logger = logger;
        }

        public async Task<ActivityResponseDto> GetActivityAsync(int id)
        {
            _logger.LogInformation("Fetching activity with ID: {ActivityId}", id);
            var activity = await _activityRepo.GetByIdAsync(id);
            if (activity == null)
            {
                _logger.LogWarning("Activity with ID {ActivityId} not found.", id);
                throw new NotFoundException($"Activity with ID {id} not found.");
            }
            return MapToActivityResponseDto(activity);
        }

        public async Task<List<ActivityResponseDto>> GetAllActivitiesAsync()
        {
            _logger.LogInformation("Fetching all activities.");
            var activities = await _activityRepo.GetAllAsync();
            return activities.Select(MapToActivityResponseDto).ToList();
        }

        public async Task<List<ActivityResponseDto>> GetActivitiesByLessonIdAsync(int lessonId)
        {
            _logger.LogInformation("Fetching activities for Lesson ID: {LessonId}", lessonId);
            var lessonExists = await _lessonRepo.GetByIdAsync(lessonId) != null;
            if (!lessonExists)
            {
                _logger.LogWarning("Lesson with ID {LessonId} not found.", lessonId);
                throw new NotFoundException($"Lesson with ID {lessonId} not found.");
            }
            var activities = await _activityRepo.GetByLessonIdAsync(lessonId);
            return activities.Select(MapToActivityResponseDto).ToList();
        }

        public async Task<ActivityResponseDto> CreateActivityAsync(ActivityRequestDto dto)
        {
            _logger.LogInformation(
                "Starting CreateActivityAsync with DTO: LessonId={LessonId}, ActivityTypeId={ActivityTypeId}, MainActivityId={MainActivityId}, SequenceOrder={SequenceOrder}",
                dto.LessonId,
                dto.ActivityTypeId,
                dto.MainActivityId,
                dto.SequenceOrder
            );

            // --- STEP 1: VALIDATE FOREIGN KEY EXISTENCE ---
            var lessonExists = await _lessonRepo.GetByIdAsync(dto.LessonId) != null;
            if (!lessonExists)
            {
                _logger.LogError(
                    "Validation failed: Lesson with ID {LessonId} not found in the database.",
                    dto.LessonId
                );
                throw new NotFoundException(
                    $"Cannot create activity: Lesson with ID {dto.LessonId} not found."
                );
            }
            _logger.LogInformation(
                "Lesson with ID {LessonId} successfully validated.",
                dto.LessonId
            );

            var activityTypeExists = await _typeRepo.GetByIdAsync(dto.ActivityTypeId) != null;
            if (!activityTypeExists)
            {
                _logger.LogError(
                    "Validation failed: Activity Type with ID {ActivityTypeId} not found in the database.",
                    dto.ActivityTypeId
                );
                throw new NotFoundException(
                    $"Cannot create activity: Activity Type with ID {dto.ActivityTypeId} not found."
                );
            }
            _logger.LogInformation(
                "Activity Type with ID {ActivityTypeId} successfully validated.",
                dto.ActivityTypeId
            );

            var mainActivityExists =
                await _mainActivityRepo.GetByIdAsync(dto.MainActivityId) != null;
            if (!mainActivityExists)
            {
                _logger.LogError(
                    "Validation failed: Main Activity with ID {MainActivityId} not found in the database.",
                    dto.MainActivityId
                );
                throw new NotFoundException(
                    $"Cannot create activity: Main Activity with ID {dto.MainActivityId} not found."
                );
            }
            _logger.LogInformation(
                "Main Activity with ID {MainActivityId} successfully validated.",
                dto.MainActivityId
            );

            // --- STEP 2: VALIDATE BUSINESS RULES ---
            var sequenceOrderExists = await _activityRepo.SequenceOrderExistsAsync(
                dto.SequenceOrder
            );
            if (sequenceOrderExists)
            {
                _logger.LogError(
                    "Validation failed: Sequence order {SequenceOrder} already exists.",
                    dto.SequenceOrder
                );
                throw new BusinessValidationException(
                    $"Sequence order {dto.SequenceOrder} already exists."
                );
            }
            _logger.LogInformation("Sequence Order {SequenceOrder} is unique.", dto.SequenceOrder);

            // --- STEP 3: CREATE AND PERSIST ENTITY ---
            _logger.LogInformation(
                "All validations passed. Mapping DTO to entity and attempting to save."
            );
            var activity = new Activity
            {
                Title = dto.Title,
                SequenceOrder = dto.SequenceOrder,
                ContentJson = dto.ContentJson,
                LessonId = dto.LessonId,
                ActivityTypeId = dto.ActivityTypeId,
                MainActivityId = dto.MainActivityId,
            };

            await _activityRepo.CreateAsync(activity);
            _logger.LogInformation(
                "Activity with ID {ActivityId} created successfully.",
                activity.ActivityId
            );

            return MapToActivityResponseDto(activity);
        }

        public async Task UpdateActivityAsync(int id, ActivityRequestDto dto)
        {
            _logger.LogInformation(
                "Starting UpdateActivityAsync for Activity ID: {ActivityId}",
                id
            );
            var activity = await _activityRepo.GetByIdAsync(id);
            if (activity == null)
            {
                _logger.LogWarning("Activity with ID {ActivityId} not found for update.", id);
                throw new NotFoundException($"Activity with ID {id} not found.");
            }

            // Validate foreign key entities.
            _logger.LogInformation("Validating foreign key IDs for update.");
            var lessonExists = await _lessonRepo.GetByIdAsync(dto.LessonId) != null;
            if (!lessonExists)
            {
                _logger.LogError(
                    "Update failed: Lesson with ID {LessonId} not found.",
                    dto.LessonId
                );
                throw new NotFoundException(
                    $"Cannot update activity: Lesson with ID {dto.LessonId} not found."
                );
            }

            var activityTypeExists = await _typeRepo.GetByIdAsync(dto.ActivityTypeId) != null;
            if (!activityTypeExists)
            {
                _logger.LogError(
                    "Update failed: Activity Type with ID {ActivityTypeId} not found.",
                    dto.ActivityTypeId
                );
                throw new NotFoundException(
                    $"Cannot update activity: Activity Type with ID {dto.ActivityTypeId} not found."
                );
            }

            var mainActivityExists =
                await _mainActivityRepo.GetByIdAsync(dto.MainActivityId) != null;
            if (!mainActivityExists)
            {
                _logger.LogError(
                    "Update failed: Main Activity with ID {MainActivityId} not found.",
                    dto.MainActivityId
                );
                throw new NotFoundException(
                    $"Cannot update activity: Main Activity with ID {dto.MainActivityId} not found."
                );
            }

            _logger.LogInformation("Foreign keys for update successfully validated.");

            // Update the tracked entity's properties.
            activity.LessonId = dto.LessonId;
            activity.ActivityTypeId = dto.ActivityTypeId;
            activity.MainActivityId = dto.MainActivityId;
            activity.Title = dto.Title;
            activity.SequenceOrder = dto.SequenceOrder;
            activity.ContentJson = dto.ContentJson;

            await _activityRepo.UpdateAsync(activity);
            _logger.LogInformation("Activity with ID {ActivityId} updated successfully.", id);
        }

        public async Task DeleteActivityAsync(int id)
        {
            _logger.LogInformation("Attempting to delete activity with ID: {ActivityId}", id);
            var activity = await _activityRepo.GetByIdAsync(id);
            if (activity == null)
            {
                _logger.LogWarning("Activity with ID {ActivityId} not found for deletion.", id);
                throw new NotFoundException($"Activity with ID {id} not found.");
            }
            await _activityRepo.DeleteAsync(id);
            _logger.LogInformation("Activity with ID {ActivityId} deleted successfully.", id);
        }

        private ActivityResponseDto MapToActivityResponseDto(Activity activity)
        {
            return new ActivityResponseDto
            {
                ActivityId = activity.ActivityId,
                LessonId = activity.LessonId,
                ActivityTypeId = activity.ActivityTypeId,
                ActivityTypeName = activity.ActivityType?.Name,
                MainActivityId = activity.MainActivityId,
                MainActivityName = activity.MainActivity?.Name,
                Title = activity.Title,
                SequenceOrder = activity.SequenceOrder,
                ContentJson = activity.ContentJson,
            };
        }
    }

    public class BusinessValidationException : Exception
    {
        public BusinessValidationException(string message)
            : base(message) { }
    }
}
