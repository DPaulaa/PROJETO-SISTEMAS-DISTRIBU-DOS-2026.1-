namespace BibliotecaRosa.Repositories;

using BibliotecaRosa.Data;
using BibliotecaRosa.Models;
using BibliotecaRosa.Repositories.Interfaces;

public class PessoaRepository : IPessoaRepository
{
    private readonly AppDbContext _db;

    public PessoaRepository(AppDbContext db)
    {
        _db = db;
    }

    public IEnumerable<Pessoa> GetAll()
    {
        return _db.Pessoas.ToList();
    }

    public Pessoa? GetById(int id)
    {
        return _db.Pessoas.Find(id);
    }

    public void Add(Pessoa pessoa)
    {
        _db.Pessoas.Add(pessoa);
        _db.SaveChanges();
    }

    public void Update(Pessoa pessoa)
    {
        _db.Pessoas.Update(pessoa);
        _db.SaveChanges();
    }

    public void Remove(Pessoa pessoa)
    {
        _db.Pessoas.Remove(pessoa);
        _db.SaveChanges();
    }
}