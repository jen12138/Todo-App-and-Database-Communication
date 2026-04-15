using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using TodoApi.Exceptions;

namespace TodoApi.Middleware;

public class ErrorHandlingMiddleware
{
   private readonly IWebHostEnvironment _env;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly RequestDelegate _next; 

    public ErrorHandlingMiddleware(
        RequestDelegate next, 
        ILogger<ErrorHandlingMiddleware> logger, 
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
        catch (TodoNotFoundException ex)
        {
            _logger.LogWarning(ex, "Todo not found:{Message}", ex.Message);

            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new
             { 
                success = false,
                message = ex.Message 
            });
        }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation error: {Message}", ex.Message);

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        catch (ArgumentException ex)        
        {
            _logger.LogWarning(ex, "Bad request: {Message}", ex.Message);
            
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database update error");
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            await context.Response.WriteAsJsonAsync(new
            {
                success = false,
                message = "A database error occurred."
            });
        }
        catch (Exception ex)
        {
            if (context.Response.HasStarted)
            {
                _logger.LogError(ex,"Unhandled exception after response started");
                throw;
            }
            _logger.LogError(
                ex, 
                "Unhandled exception. TraceIdentifier:{TraceIdentifier}",
                context.TraceIdentifier);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var message = _env.IsDevelopment() 
                ? ex.Message 
                : "An unexpected error occurred.";
           await context.Response.WriteAsJsonAsync(new
            {
                success = false,
                message
            }); 
        }
    }
}
