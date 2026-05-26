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

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EmprestimoResponse>), 200)]
    public IActionResult GetAll() =>
        Ok(_emprestimoService.GetAll());

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EmprestimoResponse), 200)]
    [ProducesResponseType(404)]
    public IActionResult GetById(int id) =>
        Ok(_emprestimoService.GetById(id));

    [HttpPost("emprestar")]
    [ProducesResponseType(typeof(EmprestimoResponse), 201)]
    [ProducesResponseType(400)]
    public IActionResult Emprestar([FromBody] EmprestimoRequest request)
    {
        var emprestimo = _emprestimoService.Emprestar(request);
        return CreatedAtAction(nameof(GetById), new { id = emprestimo.Id }, emprestimo);
    }

    [HttpPut("devolver/{id:int}")]
    [ProducesResponseType(typeof(EmprestimoResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult Devolver(int id)
    {
        var emprestimo = _emprestimoService.GetById(id);
        _emprestimoService.Devolver(id);
        return Ok(emprestimo);
    }

}