// Controllers/EmprestimoController.cs
//
// CORREÇÃO BUG 7: Devolver() chamava GetById() ANTES de Devolver() e usava esse
// resultado como retorno — devolvia o estado PRÉ-devolução (DataDevolucao ainda null).
// Corrigido: usa diretamente o retorno de _emprestimoService.Devolver(), que já
// contém o estado atualizado com DataDevolucao preenchida.

using Microsoft.AspNetCore.Mvc;
using BibliotecaRosa.Models.DTOs;
using BibliotecaRosa.Services.Interfaces;

namespace BibliotecaRosa.Controllers;

[ApiController]
[Route("api/v1/emprestimos")]
[Tags("Empréstimos")]
public class EmprestimoController : ControllerBase
{
    private readonly IEmprestimoService _emprestimoService;

    public EmprestimoController(IEmprestimoService emprestimoService) =>
        _emprestimoService = emprestimoService;

    /// <summary>Lista todos os empréstimos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EmprestimoResponse>), 200)]
    public IActionResult GetAll() =>
        Ok(_emprestimoService.GetAll());

    /// <summary>Busca um empréstimo pelo ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EmprestimoResponse), 200)]
    [ProducesResponseType(404)]
    public IActionResult GetById(int id) =>
        Ok(_emprestimoService.GetById(id));

    /// <summary>Realiza um novo empréstimo de livro para uma pessoa.</summary>
    [HttpPost("emprestar")]
    [ProducesResponseType(typeof(EmprestimoResponse), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult Emprestar([FromBody] EmprestimoRequest request)
    {
        var emprestimo = _emprestimoService.Emprestar(request);
        return CreatedAtAction(nameof(GetById), new { id = emprestimo.Id }, emprestimo);
    }

    /// <summary>Registra a devolução de um empréstimo.</summary>
    [HttpPut("devolver/{id:int}")]
    [ProducesResponseType(typeof(EmprestimoResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult Devolver(int id)
    {
        // CORREÇÃO BUG 7: usa o retorno de Devolver() — já contém DataDevolucao preenchida.
        // Antes: GetById() era chamado antes, capturando o estado pré-devolução (DataDevolucao null).
        var emprestimo = _emprestimoService.Devolver(id);
        return Ok(emprestimo);
    }
}
