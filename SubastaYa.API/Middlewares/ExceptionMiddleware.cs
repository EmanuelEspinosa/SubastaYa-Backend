using Microsoft.EntityFrameworkCore;
using SubastaYa.Domain.Exceptions;
using System.Diagnostics;
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

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var correlationId = Activity.Current?.Id ?? Guid.NewGuid().ToString();
        context.Response.ContentType = "application/json";

        var statusCode = exception switch
        {
            SubastaNoEncontradaException => HttpStatusCode.NotFound,
            ConflictoConcurrenciaException or DbUpdateConcurrencyException => HttpStatusCode.Conflict,
            SaldoInsuficienteException or PujaInvalidaException or SubastaNoActivaException => HttpStatusCode.UnprocessableEntity,
            DomainException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };
        context.Response.StatusCode = (int)statusCode;

        // NUNCA exponer detalles internos: solo mensajes de negocio seguros
        var mensaje = exception switch
        {
            DomainException d => d.Message,
            DbUpdateConcurrencyException => "El recurso fue modificado concurrentemente por otra transacción. Reintente.",
            _ => $"Error interno del servidor. Referencia: {correlationId}"
        };

        _logger.LogError(exception, "[{CorrelationId}] Error {StatusCode}: {Message}",
            correlationId, (int)statusCode, exception.Message);

        var response = new
        {
            statusCode = context.Response.StatusCode,
            message = mensaje,
            errorType = exception.GetType().Name,
            correlationId
        };
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}