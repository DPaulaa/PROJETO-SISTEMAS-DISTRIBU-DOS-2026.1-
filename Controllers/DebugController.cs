// Controllers/DebugController.cs
//
// Controller separado apenas para fins de diagnóstico e testes.
// Fica aqui isolado para não misturar com as rotas de livros.

using Microsoft.AspNetCore.Mvc;
using BibliotecaRosa.Services.Interfaces;

namespace BibliotecaRosa.Controllers;

[ApiController]
[Route("api/v1/debug")]
[Tags("Debug")]
[ApiExplorerSettings(IgnoreApi = false)] // remova em produção
public class DebugController : ControllerBase
{
    private readonly IDiagnosticoService _diagnostico;

    public DebugController(IDiagnosticoService diagnostico) =>
        _diagnostico = diagnostico;

    /// <summary>Informações de diagnóstico do serviço.</summary>
    [HttpGet("info")]
    public IActionResult GetInfo() => Ok(_diagnostico.GetInfo());

    /// <summary>Simula uma exceção 500 para fins didáticos.</summary>
    [HttpGet("crash")]
    public IActionResult Crash() =>
        throw new InvalidOperationException("Simulação de erro 500 — apenas para fins pedagógicos!");
}
