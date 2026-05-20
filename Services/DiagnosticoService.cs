// Services/DiagnosticoService.cs
//
// Coleta informações sobre o estado atual do servidor (ambiente, hora, quantidade de livros etc.).
// É usado pelo DebugController para facilitar testes e monitoramento durante o desenvolvimento.
namespace BibliotecaRosa.Services;

using BibliotecaRosa.Repositories.Interfaces;
using BibliotecaRosa.Services.Interfaces;

public class DiagnosticoService : IDiagnosticoService
{
    private readonly ILivroRepository _repo;
    private readonly IWebHostEnvironment _env;

    public DiagnosticoService(ILivroRepository repo, IWebHostEnvironment env)
    {
        _repo = repo;
        _env  = env;
    }

    public object GetInfo() => new
    {
        environment = _env.EnvironmentName,
        totalLivros = _repo.GetAll().Count(),
        serverTime  = DateTime.UtcNow,
        machineName = System.Environment.MachineName,
        processId   = System.Environment.ProcessId
    };
}
