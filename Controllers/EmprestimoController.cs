using System.Security.Claims;
using BibliotecaRosa.Models.DTOs;
using BibliotecaRosa.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaRosa.Controllers;

[ApiController]
[Route("api/v1/emprestimos")]
[Tags("Emprestimos")]
[Authorize]
public class EmprestimoController : ControllerBase
{
    private readonly IEmprestimoService _emprestimoService;

    public EmprestimoController(IEmprestimoService emprestimoService)
    {
        _emprestimoService = emprestimoService;
    }

    private int ObterUsuarioLogadoId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claim))
            throw new UnauthorizedAccessException("Usuário não autenticado.");

        return int.Parse(claim);
    }

    [HttpGet("meus-emprestimos")]
    [ProducesResponseType(typeof(IEnumerable<EmprestimoResponse>), 200)]
    public IActionResult MeusEmprestimos()
    {
        var usuarioId = ObterUsuarioLogadoId();
        return Ok(_emprestimoService.GetMeusEmprestimosAtivos(usuarioId));
    }

    [HttpGet("historico")]
    [ProducesResponseType(typeof(IEnumerable<EmprestimoResponse>), 200)]
    public IActionResult Historico()
    {
        if (User.IsInRole("Admin"))
            return Ok(_emprestimoService.GetAllAdmin());

        var usuarioId = ObterUsuarioLogadoId();
        return Ok(_emprestimoService.GetHistoricoDoUsuario(usuarioId));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EmprestimoResponse), 200)]
    [ProducesResponseType(404)]
    public IActionResult GetById(int id)
    {
        return Ok(_emprestimoService.GetById(id));
    }

    [HttpPost("emprestar")]
    [ProducesResponseType(typeof(EmprestimoResponse), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult Emprestar([FromBody] EmprestimoRequest request)
    {
        if (!User.IsInRole("Admin"))
            request.UsuarioId = ObterUsuarioLogadoId();

        var emprestimo = _emprestimoService.Emprestar(request);
        return CreatedAtAction(nameof(GetById), new { id = emprestimo.Id }, emprestimo);
    }

    [HttpPut("devolver/{id:int}")]
    [ProducesResponseType(typeof(EmprestimoResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult Devolver(int id)
    {
        return Ok(_emprestimoService.Devolver(id));
    }

    [HttpGet("admin/todos")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(IEnumerable<EmprestimoResponse>), 200)]
    public IActionResult AdminTodos()
    {
        return Ok(_emprestimoService.GetAllAdmin());
    }

    [HttpPut("admin/forcar-devolucao/{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(EmprestimoResponse), 200)]
    [ProducesResponseType(404)]
    public IActionResult ForcarDevolucao(int id)
    {
        return Ok(_emprestimoService.ForcarDevolucao(id));
    }

    [HttpGet("admin/relatorio-mais-emprestados")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(IEnumerable<RelatorioLivroDto>), 200)]
    public IActionResult RelatorioMaisEmprestados()
    {
        return Ok(_emprestimoService.GetRelatorioMaisEmprestados());
    }
}