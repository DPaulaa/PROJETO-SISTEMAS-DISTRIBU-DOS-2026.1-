// Controllers/SecureController.cs
//
// Controller para endpoints que exigem autenticação.
// A verificação do token é feita pelo middleware JWT do ASP.NET Core.
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    /// <summary>Lista livros — requer token JWT válido no header Authorization: Bearer &lt;token&gt;.</summary>
    [HttpGet("livros")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(401)]
    public IActionResult GetSecureLivros()
    {
        var livros = _livroService.GetAll();
        return Ok(new
        {
            data            = livros,
            total           = livros.Count(),
            authenticatedAt = DateTime.UtcNow
        });
    }
}
