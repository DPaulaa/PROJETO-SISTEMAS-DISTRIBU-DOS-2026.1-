// Middlewares/ExceptionHandlingMiddleware.cs
//
// Intercepta qualquer erro que aconteça durante uma requisição e devolve
// uma resposta organizada ao cliente (com status HTTP e mensagem legível).
// Assim os controllers ficam limpos — sem blocos try/catch espalhados.
//
// CORREÇÃO: o arquivo original tinha dois blocos InvokeAsync aninhados —
// _next() era chamado duas vezes e o catch de ConflitoException nunca era
// alcançado. Unificado em um único bloco com todos os catches em sequência.

namespace BibliotecaRosa.Middlewares;

using BibliotecaRosa.Exceptions;

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
        catch (ArgumentException ex)
        {
            // ValidacaoEmprestimo lança ArgumentException para dados inválidos → 400
            await Responder(context, 400, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            // Livro indisponível, empréstimo já devolvido, limite de empréstimos → 400
            await Responder(context, 400, ex.Message);
        }
        catch (Exception ex)
        {
            // O detalhe do erro vai para o log interno — o cliente recebe só uma mensagem genérica
            _logger.LogError(ex, "Erro interno em {Path}", context.Request.Path);
            await Responder(context, 500, "Ocorreu um erro interno. Tente novamente.");
        }
    }

    private static Task Responder(HttpContext ctx, int status, string mensagem)
    {
        ctx.Response.StatusCode  = status;
        ctx.Response.ContentType = "application/json";
        return ctx.Response.WriteAsJsonAsync(new { status, message = mensagem });
    }
}
