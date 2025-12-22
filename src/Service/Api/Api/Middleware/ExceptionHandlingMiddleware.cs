using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Exceptions;

namespace Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.StatusCode = ex switch
        {
            ArgumentOutOfRangeException or ArgumentNullException or ArgumentException => StatusCodes.Status400BadRequest,
            EntityNotFoundException => StatusCodes.Status404NotFound,
            FluentValidation.ValidationException => StatusCodes.Status400BadRequest,
            InvalidOperationException => StatusCodes.Status409Conflict,
            DbUpdateException => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };
        
        _logger.LogError(ex, "Необработанное исключение: {Message}", ex.Message);
        
        var errorDetails = new
        {
            Type = ex.GetType().Name,
            Code = ex.HResult,
            Message = ex.Message,

            StackTrace = _env.IsDevelopment() ? ex.StackTrace : null,
            Data = _env.IsDevelopment() ? ex.Data : null
        };
        
        context.Response.ContentType = "application/json";
        var json = JsonSerializer.Serialize(errorDetails, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        await context.Response.WriteAsync(json);
    }
}