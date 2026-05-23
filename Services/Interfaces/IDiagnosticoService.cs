// Services/Interfaces/IDiagnosticoService.cs
//
// Contrato do serviço de diagnóstico. Retorna informações sobre o estado do servidor.
namespace BibliotecaRosa.Services.Interfaces;

public interface IDiagnosticoService
{
    object GetInfo();
}
