// Services/AuthService.cs
//
// Serviço responsável por verificar se uma requisição está autorizada.
// Lê o token do cabeçalho e compara com o valor configurado no appsettings.
// Se precisarmos trocar para JWT no futuro, criamos outro serviço com o mesmo contrato (IAuthService) — sem alterar controllers ou outros serviços.
namespace BibliotecaRosa.Services;

using BibliotecaRosa.Services.Interfaces;

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IConfiguration config, ILogger<AuthService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public bool IsAuthorized(string? authHeader)
    {
        // O token válido vem do arquivo de configuração (appsettings.json), não do código-fonte
        var validToken = _config["Auth:Token"]
            ?? throw new InvalidOperationException("Auth:Token não configurado.");

        if (string.IsNullOrWhiteSpace(authHeader))
        {
            _logger.LogWarning("Requisição sem header de autorização.");
            return false;
        }

        var token = authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase)
            ? authHeader["Basic ".Length..].Trim()
            : authHeader.Trim();

        var autorizado = token == validToken;
        if (!autorizado)
            _logger.LogWarning("Token inválido recebido.");

        return autorizado;
    }
}
