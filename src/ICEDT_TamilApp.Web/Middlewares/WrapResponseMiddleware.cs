﻿using System;
using System.Net;
using System.Security.Authentication;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.S3;
using ICEDT_TamilApp.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ICEDT_TamilApp.Web.Middlewares
{
    public class WrapResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public WrapResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;

            using var newBodyStream = new MemoryStream();
            context.Response.Body = newBodyStream;

            try
            {
                await _next(context);

                if (context.Response.StatusCode == 404)
                    throw new NotFoundException("The URL is not specified or invalid.");
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, originalBodyStream);
                return;
            }

            context.Response.Body = originalBodyStream;
            newBodyStream.Seek(0, SeekOrigin.Begin);

            if (IsFileType(context.Response.ContentType))
            {
                context.Response.ContentLength = newBodyStream.Length;
                newBodyStream.Seek(0, SeekOrigin.Begin);
                await newBodyStream.CopyToAsync(context.Response.Body);
            }
            else
            {
                var responseBody = await new StreamReader(newBodyStream).ReadToEndAsync();
                var response = new Response();

                try
                {
                    response.Result = string.IsNullOrEmpty(responseBody)
                        ? null
                        : JsonSerializer.Deserialize<object>(responseBody);
                }
                catch
                {
                    response.Result = responseBody;
                }

                var wrappedResponseBody = JsonSerializer.Serialize(response);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(wrappedResponseBody);
            }
        }

        private async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception,
            Stream originalBodyStream
        )
        {
            (string Detail, string Title, int StatusCode) details = exception switch
            {
                UnauthorizedAccessException => (
                    exception.Message,
                    exception.GetType().Name,
                    StatusCodes.Status403Forbidden
                ),
                AuthenticationException => (
                    exception.Message,
                    exception.GetType().Name,
                    StatusCodes.Status401Unauthorized
                ),
                BadRequestException => (
                    exception.Message,
                    exception.GetType().Name,
                    StatusCodes.Status400BadRequest
                ),
                NotFoundException => (
                    exception.Message,
                    exception.GetType().Name,
                    StatusCodes.Status404NotFound
                ),
                ValidationException => (
                    exception.Message,
                    exception.GetType().Name,
                    StatusCodes.Status400BadRequest
                ),
                ConflictException => (
                    exception.Message,
                    exception.GetType().Name,
                    StatusCodes.Status400BadRequest
                ),

                DbUpdateException => (
                    "A database update error occurred.",
                    "DbUpdateException",
                    StatusCodes.Status400BadRequest
                ),
                AmazonS3Exception s3Ex when s3Ex.Message.Contains("not authorized") => (
                    s3Ex.Message,
                    "AmazonS3Exception",
                    StatusCodes.Status403Forbidden
                ),
                AmazonS3Exception s3Ex => (
                    $"S3 error: {s3Ex.Message}",
                    "AmazonS3Exception",
                    StatusCodes.Status500InternalServerError
                ),
                _ => (
                    exception.Message ?? "An error occurred while processing your request.",
                    exception.GetType().Name,
                    StatusCodes.Status500InternalServerError
                ),
            };

            var extensions = new Dictionary<string, object?>
            {
                { "traceId", context.TraceIdentifier },
            };

            if (exception is ValidationException validationException)
            {
                extensions.Add("ValidationErrors", validationException.Errors);
            }

            var error = new Error(details.Title, details.Detail, details.StatusCode)
            {
                Extensions = extensions,
            };

            context.Response.Body = originalBodyStream;
            var wrappedResponseBody = JsonSerializer.Serialize(new Response(null, true, error));
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = details.StatusCode;
            await context.Response.WriteAsync(wrappedResponseBody);
        }

        private bool IsFileType(string? contentType)
        {
            if (string.IsNullOrEmpty(contentType))
                return false;

            var fileTypes = new List<string>
            {
                "application/pdf",
                "image/jpeg",
                "image/png",
                "image/gif",
            };

            return fileTypes.Contains(contentType);
        }
    }

    public static class WrapResponseMiddlewareExtensions
    {
        public static IApplicationBuilder UseWrapResponseMiddleware(
            this IApplicationBuilder builder
        )
        {
            return builder.UseMiddleware<WrapResponseMiddleware>();
        }
    }
}
