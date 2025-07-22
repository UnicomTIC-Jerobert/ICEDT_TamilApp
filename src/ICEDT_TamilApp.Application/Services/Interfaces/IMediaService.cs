using ICEDT_TamilApp.Application.DTOs.Requst;
using ICEDT_TamilApp.Application.DTOs.Response;

namespace ICEDT_TamilApp.Application.Services.Interfaces
{
    public interface IMediaService
    {
        Task<MediaUploadResponseDto> UploadAsync(MediaUploadRequestDto request);
        Task DeleteAsync(string key);

        Task<MediaUrlResponseDto> GetPresignedUrlAsync(MediaUrlRequestDto request);
        Task<string> GetPublicUrlAsync(string key);

        Task<MediaListResponseDto> ListAsync(string folder);
    }
}
