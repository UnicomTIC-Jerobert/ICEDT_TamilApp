using System.Threading.Tasks;
using ICEDT_TamilApp.Application.DTOs;
using ICEDT_TamilApp.Application.DTOs.Request;
using ICEDT_TamilApp.Application.DTOs.Response;
using ICEDT_TamilApp.Application.Exceptions;
using ICEDT_TamilApp.Application.Services.Interfaces;
using ICEDT_TamilApp.Domain.Interfaces;

namespace ICEDT_TamilApp.Application.Services.Implementation
{
    public class ProgressService : IProgressService
    {
        private readonly IProgressRepository _progressRepository;
        private readonly IActivityRepository _activityRepository; // We'll need this

        private readonly ILessonRepository _lessonRepository;

        public ProgressService(
            IProgressRepository progressRepository,
            IActivityRepository activityRepository,
            ILessonRepository lessonRepository
        )
        {
            _progressRepository = progressRepository;
            _activityRepository = activityRepository;
            _lessonRepository = lessonRepository;
        }

        public async Task<CurrentLessonResponseDto?> GetCurrentLessonForUserAsync(int userId)
        {
            var currentProgress = await _progressRepository.GetCurrentProgressAsync(userId);
            int lessonIdToFetch;

            if (currentProgress == null)
            {
                // New user case: find the very first lesson
                var firstLesson = await _progressRepository.GetFirstLessonAsync();
                if (firstLesson == null)
                {
                    // This means there is no content in the DB at all.
                    throw new NotFoundException(
                        "No lessons found in the system. Cannot set initial progress."
                    );
                }

                await _progressRepository.CreateInitialProgressAsync(userId, firstLesson.LessonId);
                lessonIdToFetch = firstLesson.LessonId;
            }
            else
            {
                lessonIdToFetch = currentProgress.CurrentLessonId;
            }

            var lesson = await _lessonRepository.GetByIdAsync(lessonIdToFetch);
            if (lesson == null)
                return null;

            // Map the entity to our DTO
            var response = new CurrentLessonResponseDto
            {
                LessonId = lesson.LessonId,
                LessonName = lesson.LessonName,
                Description = lesson.Description,
                Activities = lesson
                    .Activities.Select(a => new ActivityResponseDto
                    {
                        ActivityId = a.ActivityId,
                        ActivityTypeId = a.ActivityTypeId,
                        Title = a.Title,
                        SequenceOrder = a.SequenceOrder,
                        ContentJson = a.ContentJson,
                    })
                    .OrderBy(a => a.SequenceOrder)
                    .ToList(),
            };

            return response;
        }

        public async Task<ActivityCompletionResponseDto> CompleteActivityAsync(
            int userId,
            ActivityCompletionRequestDto request
        )
        {
            // 1. Mark the activity as complete in the database
            await _progressRepository.MarkActivityAsCompleteAsync(
                userId,
                request.ActivityId,
                request.Score
            );

            // 2. Get the lesson this activity belongs to
            var activity = await _progressRepository.GetActivityByIdAsync(request.ActivityId);
            if (activity == null)
            {
                throw new NotFoundException(
                    $"Activity with ID {request.ActivityId} and name {nameof(activity)}not found."
                );
            }
            int currentLessonId = activity.LessonId;

            // 3. Check if the entire lesson is now complete
            int totalActivities = await _progressRepository.GetTotalActivitiesForLessonAsync(
                currentLessonId
            );
            int completedActivities = await _progressRepository.GetCompletedActivitiesCountAsync(
                userId,
                currentLessonId
            );

            if (completedActivities < totalActivities)
            {
                return new ActivityCompletionResponseDto
                {
                    IsLessonCompleted = false,
                    IsCourseCompleted = false,
                    Message = "Activity marked complete. Keep going!",
                };
            }

            // 4. If lesson is complete, find the next lesson
            var nextLesson = await _progressRepository.GetNextLessonAsync(currentLessonId);

            if (nextLesson == null)
            {
                // User has finished the entire course!
                return new ActivityCompletionResponseDto
                {
                    IsLessonCompleted = true,
                    IsCourseCompleted = true,
                    Message = "Congratulations! You have completed the entire course!",
                };
            }

            // 5. Unlock the next lesson for the user
            await _progressRepository.UpdateCurrentLessonAsync(userId, nextLesson.LessonId);

            return new ActivityCompletionResponseDto
            {
                IsLessonCompleted = true,
                IsCourseCompleted = false,
                Message = $"Lesson completed! You have unlocked: {nextLesson.LessonName}",
            };
        }
    }
}
