using BibliotecaRosa.Models.DTOs;
using BibliotecaRosa.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaRosa.Controllers;

[ApiController]
[Route("api/v1/usuarios")]
[Tags("Usuarios")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(IEnumerable<UsuarioRespostaDto>), 200)]
    public async Task<IActionResult> ObterTodos()
    {
        var usuarios = await _usuarioService.ObterTodosAsync();
        return Ok(usuarios);
    }

    [HttpPost]
    [ProducesResponseType(typeof(UsuarioRespostaDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> Cadastrar([FromBody] UsuarioCadastroDto dto)
    {
        var novoUsuario = await _usuarioService.CadastrarAsync(dto);
        return CreatedAtAction(nameof(ObterTodos), new { id = novoUsuario.Id }, novoUsuario);
    }

    [HttpPut("{id:int}")]
[Authorize(Roles = "Admin")]
[ProducesResponseType(typeof(UsuarioRespostaDto), 200)]
[ProducesResponseType(404)]
public async Task<IActionResult> Atualizar(int id, [FromBody] UsuarioAtualizacaoDto dto)
{
    var usuario = await _usuarioService.AtualizarAsync(id, dto);
    return Ok(usuario);
}

[HttpDelete("{id:int}")]
[Authorize(Roles = "Admin")]
[ProducesResponseType(204)]
[ProducesResponseType(404)]
public async Task<IActionResult> Remover(int id)
{
    await _usuarioService.RemoverAsync(id);
    return NoContent();
}
}