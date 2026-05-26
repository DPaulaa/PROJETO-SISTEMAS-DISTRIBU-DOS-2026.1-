namespace BibliotecaRosa.Repositories.Interfaces;

using BibliotecaRosa.Models;

public interface IEmprestimoRepository
{
    IEnumerable<Emprestimo> GetAll();
    Emprestimo? GetById(int id);
    List<Emprestimo> GetByPessoaId(int pessoaId);
    List<Emprestimo> GetByLivroAndPessoa(int livroId, int pessoaId);
    Emprestimo Add(Emprestimo emprestimo);
    Emprestimo Update(Emprestimo emprestimo);
    void Remove(Emprestimo emprestimo);
}