using BibliotecaRosa.Models.DTOs;
using BibliotecaRosa.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BibliotecaRosa.Controllers;

[ApiController]
[Route("api/v1/usuarios")]
[Tags("Usuários")]
[Authorize]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly IEmprestimoService _emprestimoService;

    public UsuariosController(IUsuarioService usuarioService, IEmprestimoService emprestimoService)
    {
        _usuarioService = usuarioService;
        _emprestimoService = emprestimoService;
    }

    /// <summary>Lista todos os usuários (somente Admin).</summary>
    [HttpGet]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(IEnumerable<UsuarioRespostaDto>), 200)]
    public async Task<IActionResult> ObterTodos()
    {
        var usuarios = await _usuarioService.ObterTodosAsync();
        return Ok(usuarios);
    }

    /// <summary>Busca um usuário por ID (Admin ou o próprio usuário).</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(UsuarioRespostaDto), 200)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var usuarioLogadoId = ObterUsuarioLogadoId();
        var isAdmin = User.IsInRole("Administrador");

        if (!isAdmin && usuarioLogadoId != id)
            return Forbid();

        var usuario = await _usuarioService.ObterPorIdAsync(id);
        return Ok(usuario);
    }

    /// <summary>Atualiza dados de um usuário (Admin ou o próprio usuário).</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(UsuarioRespostaDto), 200)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] UsuarioAtualizacaoDto dto)
    {
        var usuarioLogadoId = ObterUsuarioLogadoId();
        var isAdmin = User.IsInRole("Administrador");

        if (!isAdmin && usuarioLogadoId != id)
            return Forbid();

        var atualizado = await _usuarioService.AtualizarAsync(id, dto);
        return Ok(atualizado);
    }

    /// <summary>Remove um usuário (somente Admin).</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Remover(int id)
    {
        await _usuarioService.RemoverAsync(id);
        return NoContent();
    }

    /// <summary>Lista empréstimos de um usuário específico (somente Admin).</summary>
    [HttpGet("{id:int}/emprestimos")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(IEnumerable<EmprestimoResponse>), 200)]
    public IActionResult EmprestimosPorUsuario(int id)
    {
        var emprestimos = _emprestimoService.GetByUsuario(id);
        return Ok(emprestimos);
    }

    private int ObterUsuarioLogadoId()
    {
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier)
               ?? User.FindFirstValue("sub")
               ?? "0";
        return int.TryParse(sub, out var id) ? id : 0;
    }
}
