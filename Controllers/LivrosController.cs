// Controllers/LivrosController.cs
//
// Este controller é a "porta de entrada" da API para tudo relacionado a livros.
// Ele só recebe a requisição HTTP, chama quem sabe fazer o trabalho (o serviço) e devolve a resposta. Nenhuma regra de negócio fica aqui.
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

    // O serviço é recebido automaticamente pelo ASP.NET — não precisamos criá-lo manualmente.
    public LivrosController(ILivroService livroService) =>
        _livroService = livroService;

    /// <summary>Lista todos os livros da biblioteca.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<LivroResponse>), 200)]
    public IActionResult GetAll() =>
        Ok(_livroService.GetAll());

    /// <summary>Busca um livro pelo ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(LivroResponse), 200)]
    [ProducesResponseType(404)]
    public IActionResult GetById(int id) =>
        Ok(_livroService.GetById(id)); // se não encontrar, o middleware devolve 404 automaticamente

    /// <summary>Adiciona um novo livro à biblioteca.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(LivroResponse), 201)]
    [ProducesResponseType(400)]
    public IActionResult Create([FromBody] LivroRequest request)
    {
        var livro = _livroService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = livro.Id }, livro);
    }

    /// <summary>Atualiza os dados de um livro existente.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(LivroResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult Update(int id, [FromBody] LivroRequest request) =>
        Ok(_livroService.Update(id, request));

    /// <summary>Remove um livro da biblioteca.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult Delete(int id)
    {
        _livroService.Delete(id);
        return NoContent();
    }
}
