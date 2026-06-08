using BibliotecaRosa.Models.DTOs;
using BibliotecaRosa.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaRosa.Controllers;

[ApiController]
[Route("api/v1/auth")]
[Tags("Autenticação")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) => _authService = authService;

    /// <summary>Autentica um usuário e retorna um token JWT.</summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var resultado = await _authService.LoginAsync(request);
        return Ok(resultado);
    }

    /// <summary>Registra um novo usuário no sistema.</summary>
    [HttpPost("registrar")]
    [ProducesResponseType(typeof(UsuarioRespostaDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> Registrar(
        [FromBody] UsuarioCadastroDto dto,
        [FromServices] IUsuarioService usuarioService)
    {
        var novoUsuario = await usuarioService.CadastrarAsync(dto);
        return CreatedAtAction(nameof(Login), novoUsuario);
    }
}
