// Controllers/SecureController.cs
//
// Controller para endpoints que exigem autenticação.
// Quem quiser acessar precisa enviar um token no cabeçalho da requisição.
// A verificação do token é feita pelo AuthService.
using Microsoft.AspNetCore.Mvc;
using BibliotecaRosa.Models.DTOs;
using BibliotecaRosa.Services.Interfaces;

namespace BibliotecaRosa.Controllers;

[ApiController]
[Route("api/v1/secure")]
[Tags("Protegido")]
public class SecureController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILivroService _livroService;

    public SecureController(IAuthService authService, ILivroService livroService)
    {
        _authService  = authService;
        _livroService = livroService;
    }

    /// <summary>Lista livros — requer header Authorization: Basic &lt;token&gt;.</summary>
    [HttpGet("livros")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(401)]
    public IActionResult GetSecureLivros()
    {
        // Lê o token do cabeçalho e pergunta ao AuthService se ele é válido
        var authHeader = Request.Headers.Authorization.ToString();

        if (!_authService.IsAuthorized(authHeader))
            return StatusCode(401, new
            {
                message = "Acesso negado. Envie: Authorization: Basic <token>"
            });

        var livros = _livroService.GetAll();
        return Ok(new
        {
            data            = livros,
            total           = livros.Count(),
            authenticatedAt = DateTime.UtcNow
        });
    }
}
