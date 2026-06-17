using BibliotecaRosa.Models;

namespace BibliotecaRosa.Repositories.Interfaces;

public interface IEmprestimoRepository
{
    IEnumerable<Emprestimo> GetAll();
    Emprestimo? GetById(int id);
    IEnumerable<Emprestimo> GetByUsuarioId(int usuarioId);
    IEnumerable<Emprestimo> GetEmprestimosAtivosPorUsuario(int usuarioId);
    IEnumerable<Emprestimo> GetEmprestimosAtivosPorLivro(int livroId);
    void Add(Emprestimo emprestimo);
    void Update(Emprestimo emprestimo);
    void Remove(Emprestimo emprestimo);
}