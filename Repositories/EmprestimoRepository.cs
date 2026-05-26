namespace BibliotecaRosa.Repositories;

using BibliotecaRosa.Data;
using BibliotecaRosa.Models;
using BibliotecaRosa.Repositories.Interfaces;

public class EmprestimoRepository : IEmprestimoRepository
{
    private readonly AppDbContext _db;

    public EmprestimoRepository(AppDbContext db)
    {
        _db = db;
    }

    public IEnumerable<Emprestimo> GetAll()
    {
        return _db.Emprestimos.ToList();
    }

    public Emprestimo? GetById(int id)
    {
        return _db.Emprestimos.Find(id);
    }

    public Emprestimo Add(Emprestimo emprestimo)
    {
        _db.Emprestimos.Add(emprestimo);
        _db.SaveChanges();
        return emprestimo;
    }

    public Emprestimo Update(Emprestimo emprestimo)
    {
        _db.Emprestimos.Update(emprestimo);
        _db.SaveChanges();
        return emprestimo;
    }

    public void Remove(Emprestimo emprestimo)
    {
        _db.Emprestimos.Remove(emprestimo);
        _db.SaveChanges();
    }

    public List<Emprestimo> GetByPessoaId(int pessoaId)
    {
        return _db.Emprestimos.Where(e => e.Pessoa.Id == pessoaId).ToList();
    }

    public List<Emprestimo> GetByLivroAndPessoa(int livroId, int pessoaId)
    {
        return _db.Emprestimos.Where(e => e.Livro.Id == livroId && e.Pessoa.Id == pessoaId).ToList();
    }
}