namespace BibliotecaRosa.Repositories.Interfaces;

using BibliotecaRosa.Models;

public interface IPessoaRepository
{
    IEnumerable<Pessoa> GetAll();
    Pessoa? GetById(int id);
    void Add(Pessoa pessoa);
    void Update(Pessoa pessoa);
    void Remove(Pessoa pessoa);
}