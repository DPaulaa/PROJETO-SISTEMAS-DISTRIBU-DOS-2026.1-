namespace BibliotecaRosa.Repositories.Interfaces;

using BibliotecaRosa.Models;
using BibliotecaRosa.Models.DTOs;

public interface IEmprestimoRepository
{
    IEnumerable<Emprestimo> GetAll();
    Emprestimo? GetById(int id);
    List<Emprestimo> GetByUsuarioId(int usuarioId);
    List<Emprestimo> GetByLivroAndUsuario(int livroId, int usuarioId);
    List<Emprestimo> GetAtivos();
    List<RelatorioLivroDto> GetRelatorioMaisEmprestados();
    Emprestimo Add(Emprestimo emprestimo);
    Emprestimo Update(Emprestimo emprestimo);
    void Remove(Emprestimo emprestimo);
}
