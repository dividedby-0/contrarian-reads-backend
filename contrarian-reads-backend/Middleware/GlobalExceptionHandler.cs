using System.Diagnostics;
using contrarian_reads_backend.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace contrarian_reads_backend.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly IHostEnvironment _environment;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger,
        IHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var errorResponse = new ErrorResponse
        {
            TraceId = Activity.Current?.Id ?? httpContext.TraceIdentifier
        };

        (errorResponse.StatusCode, errorResponse.Message) = exception switch
        {
            DbUpdateException dbEx => (500, "Database error occurred."),
            SqlException sqlEx => (500, "Database error occurred."),
            UnauthorizedAccessException => (401, "Unauthorized access."),
            NotFoundException notFoundEx => (404, notFoundEx.Message),
            InvalidOperationException invEx => (400, invEx.Message),
            _ => (500, "An unexpected error occurred.")
        };

        _logger.LogError(exception,
            "Error {TraceId}: {Message}",
            errorResponse.TraceId,
            exception.Message);

        if (_environment.IsDevelopment()) errorResponse.DeveloperMessage = exception.ToString();

        httpContext.Response.StatusCode = errorResponse.StatusCode;
        await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

        return true;
    }
}