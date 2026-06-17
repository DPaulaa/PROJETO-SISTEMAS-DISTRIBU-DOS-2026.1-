using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BibliotecaRosa.Models.DTOs;
using BibliotecaRosa.Services.Interfaces;

namespace BibliotecaRosa.Controllers;

[ApiController]
[Route("api/v1/secure")]
[Tags("Protegido")]
[Authorize]
public class SecureController : ControllerBase
{
    private readonly ILivroService _livroService;

    public SecureController(ILivroService livroService)
    {
        _livroService = livroService;
    }

    [HttpGet("livros")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(401)]
    public IActionResult GetSecureLivros()
    {
        var livros = _livroService.GetAll();
        return Ok(new
        {
            data = livros,
            total = livros.Count(),
            authenticatedAt = DateTime.UtcNow,
            user = User.Identity?.Name ?? "Desconhecido"
        });
    }
}