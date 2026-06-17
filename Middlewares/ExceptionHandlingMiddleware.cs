using System.Text.Json;
using BibliotecaRosa.Exceptions;

namespace BibliotecaRosa.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (RecursoNaoEncontradoException ex)
        {
            await Responder(context, 404, ex.Message);
        }
        catch (RegraDeNegocioException ex)
        {
            await Responder(context, 400, ex.Message);
        }
        catch (ConflitoException ex)
        {
            await Responder(context, 409, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno em {Path}", context.Request.Path);
            await Responder(context, 500, "Ocorreu um erro interno. Tente novamente.");
        }
    }

    private static async Task Responder(HttpContext ctx, int status, string mensagem)
    {
        ctx.Response.StatusCode = status;
        ctx.Response.ContentType = "application/json";
        await ctx.Response.WriteAsJsonAsync(new { status, message = mensagem });
    }
}