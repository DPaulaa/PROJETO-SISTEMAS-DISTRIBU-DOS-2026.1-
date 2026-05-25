namespace BibliotecaRosa.Repositories.Interfaces;

using BibliotecaRosa.Models;

public interface IEmprestimoRepository
{
    IEnumerable<Emprestimo> GetAll();
    Emprestimo? GetById(int id);
    void Add(Emprestimo emprestimo);
    void Update(Emprestimo emprestimo);
    void Remove(Emprestimo emprestimo);
}