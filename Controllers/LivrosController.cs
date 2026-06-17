using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BibliotecaRosa.Models.DTOs;
using BibliotecaRosa.Services.Interfaces;

namespace BibliotecaRosa.Controllers;

[ApiController]
[Route("api/v1/livros")]
[Tags("Livros")]
public class LivrosController : ControllerBase
{
    private readonly ILivroService _livroService;

    public LivrosController(ILivroService livroService)
    {
        _livroService = livroService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<LivroResponse>), 200)]
    public IActionResult GetAll()
    {
        return Ok(_livroService.GetAll());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(LivroResponse), 200)]
    [ProducesResponseType(404)]
    public IActionResult GetById(int id)
    {
        return Ok(_livroService.GetById(id));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(LivroResponse), 201)]
    [ProducesResponseType(400)]
    public IActionResult Create([FromBody] LivroRequest request)
    {
        var livro = _livroService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = livro.Id }, livro);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(LivroResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult Update(int id, [FromBody] LivroRequest request)
    {
        return Ok(_livroService.Update(id, request));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult Delete(int id)
    {
        _livroService.Delete(id);
        return NoContent();
    }
}