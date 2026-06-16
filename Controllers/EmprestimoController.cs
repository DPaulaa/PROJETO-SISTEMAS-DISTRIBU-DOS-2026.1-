using BibliotecaRosa.Models.DTOs;
using BibliotecaRosa.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BibliotecaRosa.Controllers;

[ApiController]
[Route("api/v1/emprestimos")]
[Tags("Empréstimos")]
[Authorize]
public class EmprestimoController : ControllerBase
{
    private readonly IEmprestimoService _emprestimoService;

    public EmprestimoController(IEmprestimoService emprestimoService) =>
        _emprestimoService = emprestimoService;

    // ── Comuns ────────────────────────────────────────────────────────────────

    /// <summary>Lista todos os empréstimos ativos do usuário logado.</summary>
    [HttpGet("meus-emprestimos")]
    [ProducesResponseType(typeof(IEnumerable<EmprestimoResponse>), 200)]
    public IActionResult MeusEmprestimos()
    {
        var usuarioId = ObterUsuarioLogadoId();
        return Ok(_emprestimoService.GetMeusEmprestimosAtivos(usuarioId));
    }

    /// <summary>Histórico completo de empréstimos: usuário vê só o seu, admin vê tudo.</summary>
    [HttpGet("historico")]
    [ProducesResponseType(typeof(IEnumerable<EmprestimoResponse>), 200)]
    public IActionResult Historico()
    {
        if (User.IsInRole("Administrador"))
            return Ok(_emprestimoService.GetAllAdmin());

        var usuarioId = ObterUsuarioLogadoId();
        return Ok(_emprestimoService.GetHistoricoDoUsuario(usuarioId));
    }

    /// <summary>Busca um empréstimo pelo ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EmprestimoResponse), 200)]
    [ProducesResponseType(404)]
    public IActionResult GetById(int id) =>
        Ok(_emprestimoService.GetById(id));

    /// <summary>Realiza um novo empréstimo para o usuário logado.</summary>
    [HttpPost("emprestar")]
    [ProducesResponseType(typeof(EmprestimoResponse), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult Emprestar([FromBody] EmprestimoRequest request)
    {
        // Se não for admin, força o UsuarioId a ser o usuário logado
        if (!User.IsInRole("Administrador"))
            request.UsuarioId = ObterUsuarioLogadoId();

        var emprestimo = _emprestimoService.Emprestar(request);
        return CreatedAtAction(nameof(GetById), new { id = emprestimo.Id }, emprestimo);
    }

    /// <summary>Registra a devolução de um empréstimo.</summary>
    [HttpPut("devolver/{id:int}")]
    [ProducesResponseType(typeof(EmprestimoResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult Devolver(int id) =>
        Ok(_emprestimoService.Devolver(id));

    // ── Admin ─────────────────────────────────────────────────────────────────

    /// <summary>Lista todos os empréstimos do sistema, incluindo devolvidos (somente Admin).</summary>
    [HttpGet("admin/todos")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(IEnumerable<EmprestimoResponse>), 200)]
    public IActionResult GetAllAdmin() =>
        Ok(_emprestimoService.GetAllAdmin());

    /// <summary>Força a devolução de um empréstimo (somente Admin).</summary>
    [HttpPut("{id:int}/forcar-devolucao")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(EmprestimoResponse), 200)]
    [ProducesResponseType(404)]
    public IActionResult ForcarDevolucao(int id) =>
        Ok(_emprestimoService.ForcarDevolucao(id));

    /// <summary>Relatório de livros mais emprestados (somente Admin).</summary>
    [HttpGet("admin/relatorio/mais-emprestados")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(IEnumerable<RelatorioLivroDto>), 200)]
    public IActionResult RelatorioMaisEmprestados() =>
        Ok(_emprestimoService.GetRelatorioMaisEmprestados());

    private int ObterUsuarioLogadoId()
    {
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier)
               ?? User.FindFirstValue("sub")
               ?? "0";
        return int.TryParse(sub, out var id) ? id : 0;
    }
}
