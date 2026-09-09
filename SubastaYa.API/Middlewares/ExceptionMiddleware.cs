using Microsoft.EntityFrameworkCore;
using SubastaYa.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace SubastaYa.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepción capturada por el middleware: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        // Mapeo preciso de códigos de estado HTTP según la rúbrica del TP
        var statusCode = exception switch
        {
            SubastaNoEncontradaException => HttpStatusCode.NotFound, // 404
            ConflictoConcurrenciaException or DbUpdateConcurrencyException => HttpStatusCode.Conflict, // 409 Conflict
            DomainException => HttpStatusCode.BadRequest, // 400 Bad Request
            _ => HttpStatusCode.InternalServerError // 500
        };

        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            statusCode = context.Response.StatusCode,
            message = exception.Message,
            errorType = exception.GetType().Name
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}