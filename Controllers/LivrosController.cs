using Microsoft.AspNetCore.Mvc;
using LivrariaRosa.Models.DTOs.Requests;
using LivrariaRosa.Models.DTOs.Responses;
using LivrariaRosa.Services.Interfaces;

namespace LivrariaRosa.Controllers;

/// <summary>Gerenciamento do acervo de livros</summary>
[ApiController]
[Route("api/v1/livros")]
[Produces("application/json")]
public class LivrosController : ControllerBase
{
    private readonly ILivroService _service;
    private readonly ILogger<LivrosController> _logger;

    public LivrosController(ILivroService service, ILogger<LivrosController> logger)
    {
        _service = service;
        _logger  = logger;
    }

    /// <summary>Lista todos os livros com paginação</summary>
    /// <param name="pagina">Número da página (padrão: 1)</param>
    /// <param name="tamanhoPagina">Itens por página (padrão: 10)</param>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanhoPagina = 10)
    {
        if (pagina < 1) pagina = 1;
        if (tamanhoPagina < 1 || tamanhoPagina > 100) tamanhoPagina = 10;

        var resultado = await _service.ListarTodosAsync(pagina, tamanhoPagina);
        return Ok(ApiResponse<object>.Ok(resultado));
    }

    /// <summary>Busca um livro pelo ID</summary>
    /// <param name="id">ID do livro</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<LivroResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var livro = await _service.BuscarPorIdAsync(id);
        if (livro is null)
            return NotFound(ApiResponse<object>.Falha($"Livro com id={id} não encontrado."));

        return Ok(ApiResponse<LivroResponse>.Ok(livro));
    }

    /// <summary>Adiciona um novo livro à biblioteca</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<LivroResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] LivroRequest request)
    {
        if (!ModelState.IsValid)
        {
            var erros = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(ApiResponse<object>.Falha("Dados inválidos.", erros));
        }

        var livro = await _service.CriarAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = livro.Id },
            ApiResponse<LivroResponse>.Ok(livro));
    }

    /// <summary>Atualiza os dados de um livro existente</summary>
    /// <param name="id">ID do livro a atualizar</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<LivroResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] LivroRequest request)
    {
        if (!ModelState.IsValid)
        {
            var erros = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(ApiResponse<object>.Falha("Dados inválidos.", erros));
        }

        var livro = await _service.AtualizarAsync(id, request);
        if (livro is null)
            return NotFound(ApiResponse<object>.Falha($"Livro com id={id} não encontrado."));

        return Ok(ApiResponse<LivroResponse>.Ok(livro));
    }

    /// <summary>Remove um livro da biblioteca (soft delete)</summary>
    /// <param name="id">ID do livro a remover</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var removido = await _service.RemoverAsync(id);
        if (!removido)
            return NotFound(ApiResponse<object>.Falha($"Livro com id={id} não encontrado."));

        return NoContent();
    }
}
