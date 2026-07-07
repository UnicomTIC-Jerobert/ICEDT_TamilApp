using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ICEDT_TamilApp.Application.Services.Interfaces
{
    public interface IFileUploader
    {
        Task<string> UploadFileAsync(IFormFile file, string s3Key, CancellationToken cancellationToken = default);
        Task DeleteFileAsync(string s3Key, CancellationToken cancellationToken = default);
    }
}
