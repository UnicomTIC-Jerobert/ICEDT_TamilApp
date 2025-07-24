using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using ICEDT_TamilApp.Application.Common;
using ICEDT_TamilApp.Application.DTOs.Requst;
using ICEDT_TamilApp.Application.DTOs.Response;
using ICEDT_TamilApp.Application.Exceptions;
using ICEDT_TamilApp.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ICEDT_TamilApp.Application.Services.Implementation
{
    public class MediaService : IMediaService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly AwsSettings _awsSettings;

        // The constructor is now clean. It just receives the dependencies it needs.
        public MediaService(IAmazonS3 s3Client, IOptions<AwsSettings> awsOptions)
        {
            _s3Client = s3Client;
            _awsSettings = awsOptions.Value; // Get the actual settings object

            // No more manual client creation or configuration reading here!
        }

        public async Task<MediaUploadResponseDto> UploadAsync(MediaUploadRequestDto request)
        {
            if (request.File == null || request.File.Length == 0)
                throw new BadRequestException("File is empty");

            // ... (validation logic remains the same) ...

            var key =
                $"{request.Folder}/{Guid.NewGuid()}_{Path.GetFileName(request.File.FileName)}";

            try
            {
                var uploadRequest = new PutObjectRequest
                {
                    BucketName = _awsSettings.BucketName, // Use the settings object
                    Key = key,
                    InputStream = request.File.OpenReadStream(),
                    ContentType = request.File.ContentType,
                    ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256,
                };

                var response = await _s3Client.PutObjectAsync(uploadRequest);

                if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new InvalidOperationException("Failed to upload file to S3");
                }

                var url = GetPublicUrl(key);

                return new MediaUploadResponseDto
                {
                    // ...
                    Url = url,
                    Message = "File uploaded successfully",
                };
            }
            catch (AmazonS3Exception ex)
            {
                throw new InvalidOperationException($"S3 Error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Upload Error: {ex.Message}", ex);
            }
        }

        public async Task DeleteAsync(string key)
        {
            // ... (Implementation is much cleaner now)
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = _awsSettings.BucketName, // Use settings
                Key = key,
            };
            await _s3Client.DeleteObjectAsync(deleteObjectRequest);
        }

        // This is now a private helper method, as the configuration is internal to the service
        private string GetPublicUrl(string key)
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;
            return $"https://{_awsSettings.BucketName}.s3.{_awsSettings.Region}.amazonaws.com/{key}";
        }

        // You can remove GetPublicUrlAsync if this helper is sufficient, or implement it like this:
        public Task<string> GetPublicUrlAsync(string key)
        {
            return Task.FromResult(GetPublicUrl(key));
        }

        public async Task<MediaListResponseDto> ListAsync(string folder)
        {
            try
            {
                var request = new ListObjectsV2Request
                {
                    BucketName = _awsSettings.BucketName,
                    Prefix = string.IsNullOrEmpty(folder) ? "" : $"{folder}/",
                    MaxKeys = 1000,
                };

                var response = await _s3Client.ListObjectsV2Async(request);
                var keys = new List<string>();

                foreach (var obj in response.S3Objects)
                {
                    keys.Add(obj.Key);
                }

                return new MediaListResponseDto
                {
                    Files = keys,
                    Count = keys.Count,
                    Folder = folder,
                };
            }
            catch (AmazonS3Exception ex)
            {
                throw new InvalidOperationException($"S3 List Error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"List Error: {ex.Message}", ex);
            }
        }

        public Task<MediaUrlResponseDto> GetPresignedUrlAsync(MediaUrlRequestDto request)
        {
            if (string.IsNullOrEmpty(request.Key))
                throw new BadRequestException("Key cannot be empty");

            try
            {
                var presignedRequest = new GetPreSignedUrlRequest
                {
                    BucketName = _awsSettings.BucketName,
                    Key = request.Key,
                    Expires = DateTime.UtcNow.AddMinutes(request.ExpiryMinutes),
                    Verb = HttpVerb.GET,
                };

                var url = _s3Client.GetPreSignedURL(presignedRequest);

                return Task.FromResult(
                    new MediaUrlResponseDto
                    {
                        Url = url,
                        Key = request.Key,
                        ExpiryMinutes = request.ExpiryMinutes,
                    }
                );
            }
            catch (AmazonS3Exception ex)
            {
                throw new InvalidOperationException($"S3 PreSigned URL Error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"PreSigned URL Error: {ex.Message}", ex);
            }
        }

        // ... (The rest of your methods like ListAsync, GetPresignedUrlAsync also use _awsSettings.BucketName)

        // The Dispose method is no longer needed in this service, as the DI container
        // will manage the lifetime of the IAmazonS3 client.
    }
}
