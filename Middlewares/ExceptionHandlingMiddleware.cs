using System.Net;
using System.Text.Json;
using LivrariaRosa.Models.DTOs.Responses;

namespace LivrariaRosa.Middlewares;

/// <summary>
/// Middleware global de tratamento de exceções.
/// Impede que stack traces ou detalhes internos cheguem ao cliente.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next   = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Recurso não encontrado");
            await EscreverResposta(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Argumento inválido");
            await EscreverResposta(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            // Loga o erro completo internamente, mas não expõe ao cliente
            _logger.LogError(ex, "Erro não tratado na requisição {Method} {Path}",
                context.Request.Method, context.Request.Path);

            await EscreverResposta(context, HttpStatusCode.InternalServerError,
                "Ocorreu um erro interno. Tente novamente em instantes.");
        }
    }

    private static async Task EscreverResposta(HttpContext context, HttpStatusCode statusCode, string mensagem)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode  = (int)statusCode;

        var resposta = ApiResponse<object>.Falha(mensagem);
        var json     = JsonSerializer.Serialize(resposta, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
