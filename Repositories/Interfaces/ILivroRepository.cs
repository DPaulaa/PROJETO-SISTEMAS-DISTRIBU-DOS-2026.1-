using LivrariaRosa.Models.Entities;

namespace LivrariaRosa.Repositories.Interfaces;

public interface ILivroRepository
{
    Task<IEnumerable<Livro>> ListarTodosAsync(int pagina, int tamanhoPagina);
    Task<int> ContarTotalAsync();
    Task<Livro?> BuscarPorIdAsync(int id);
    Task AdicionarAsync(Livro livro);
    Task AtualizarAsync(Livro livro);
    Task RemoverAsync(Livro livro);
}
