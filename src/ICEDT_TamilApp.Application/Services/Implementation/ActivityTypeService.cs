using ICEDT_TamilApp.Application.DTOs.Request;
using ICEDT_TamilApp.Application.DTOs.Response;
using ICEDT_TamilApp.Application.Exceptions;
using ICEDT_TamilApp.Application.Services.Interfaces;
using ICEDT_TamilApp.Domain.Entities;
using ICEDT_TamilApp.Domain.Interfaces;

namespace ICEDT_TamilApp.Application.Services.Implementation
{
    public class ActivityTypeService : IActivityTypeService
    {
        private readonly IActivityRepository _activityRepo;
        private readonly IActivityTypeRepository _activityTypeRepo;

        public ActivityTypeService(IActivityRepository activityRepo, IActivityTypeRepository typeRepo)
        {
            _activityRepo = activityRepo;
            this._activityTypeRepo = typeRepo;
        }


        // ActivityType CRUD
        public async Task<ActivityTypeResponseDto> GetActivityTypeAsync(int id)
        {
            var type = await this._activityTypeRepo.GetByIdAsync(id);
            if (type == null)
                throw new NotFoundException("ActivityType not found.");
            return MapToActivityTypeResponseDto(type);
        }

        public async Task<List<ActivityTypeResponseDto>> GetAllActivityTypesAsync()
        {
            var types = await this._activityTypeRepo.GetAllAsync();
            return types.Select(MapToActivityTypeResponseDto).ToList();
        }

        public async Task<ActivityTypeResponseDto> AddActivityTypeAsync(ActivityTypeRequestDto dto)
        {
            var type = new ActivityType
            {
                Name = dto.ActivityName,

            };
            await this._activityTypeRepo.CreateAsync(type);
            return MapToActivityTypeResponseDto(type);
        }

        public async Task UpdateActivityTypeAsync(int id, ActivityTypeRequestDto dto)
        {
            var type = await this._activityTypeRepo.GetByIdAsync(id);
            if (type == null)
                throw new NotFoundException("ActivityType not found.");
            type.Name = dto.ActivityName;
            await this._activityTypeRepo.UpdateAsync(type);
        }

        public async Task DeleteActivityTypeAsync(int id)
        {
            var type = await this._activityTypeRepo.GetByIdAsync(id);
            if (type == null)
                throw new NotFoundException("ActivityType not found.");
            var hasActivities = await _activityRepo
                .GetAllAsync()
                .ContinueWith(t => t.Result.Any(a => a.ActivityTypeId == id));
            if (hasActivities)
                throw new BadRequestException(
                    "Cannot delete ActivityType with associated Activities."
                );
            await this._activityTypeRepo.DeleteAsync(id);
        }


        private ActivityTypeResponseDto MapToActivityTypeResponseDto(ActivityType type)
        {
            return new ActivityTypeResponseDto
            {
                ActivityTypeId = type.ActivityTypeId,
                ActivityName = type.Name,
            };
        }
    }
}
