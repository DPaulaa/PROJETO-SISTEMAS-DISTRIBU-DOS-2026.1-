using BibliotecaRosa.Models;

namespace BibliotecaRosa.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> BuscarTodosAsync();
        Task<Usuario?> BuscarPorIdAsync(int id);
        Task<Usuario?> BuscarPorEmailAsync(string email);
        Task AdicionarAsync(Usuario usuario);
    }
}