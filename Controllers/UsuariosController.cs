// Controllers/UsuariosController.cs
//
// CORREÇÃO BUG 6: a rota estava como "api/[controller]" (resolvia para "api/Usuarios")
// enquanto todos os outros controllers usam o padrão explícito "api/v1/...".
// Corrigido para "api/v1/usuarios" + adicionada tag do Swagger e ProducesResponseType.

using BibliotecaRosa.Models.DTOs;
using BibliotecaRosa.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaRosa.Controllers;

[ApiController]
[Route("api/v1/usuarios")]
[Tags("Usuários")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService) =>
        _usuarioService = usuarioService;

    /// <summary>Lista todos os usuários cadastrados.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UsuarioRespostaDto>), 200)]
    public async Task<IActionResult> ObterTodos()
    {
        var usuarios = await _usuarioService.ObterTodosAsync();
        return Ok(usuarios);
    }

    /// <summary>Cadastra um novo usuário.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(UsuarioRespostaDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> Cadastrar([FromBody] UsuarioCadastroDto dto)
    {
        var novoUsuario = await _usuarioService.CadastrarAsync(dto);
        return CreatedAtAction(nameof(ObterTodos), new { id = novoUsuario.Id }, novoUsuario);
    }
}
