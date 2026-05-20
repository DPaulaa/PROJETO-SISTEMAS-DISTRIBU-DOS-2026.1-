using LivrariaRosa.Models.DTOs.Requests;
using LivrariaRosa.Models.DTOs.Responses;

namespace LivrariaRosa.Services.Interfaces;

public interface ILivroService
{
    Task<object> ListarTodosAsync(int pagina, int tamanhoPagina);
    Task<LivroResponse?> BuscarPorIdAsync(int id);
    Task<LivroResponse> CriarAsync(LivroRequest request);
    Task<LivroResponse?> AtualizarAsync(int id, LivroRequest request);
    Task<bool> RemoverAsync(int id);
}
