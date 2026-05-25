namespace BibliotecaRosa.Repositories;

using BibliotecaRosa.Data;
using BibliotecaRosa.Models;
using BibliotecaRosa.Repositories.Interfaces;

public class EmprestimoRepository : IEmprestimoRepository
{
    private readonly AppDbContext _db;
    private int _nextId = 1;
    private readonly object _lock = new();

    public IEnumerable<Emprestimo> GetAll()
    {
        return _db.Emprestimos.ToList();
    }

    public Emprestimo? GetById(int id)
    {
        return _db.Emprestimos.Find(id);
    }

    public void Add(Emprestimo emprestimo)
    {
        _db.Emprestimos.Add(emprestimo);
        _db.SaveChanges();
    }

    public void Update(Emprestimo emprestimo)
    {
        _db.Emprestimos.Update(emprestimo);
        _db.SaveChanges();
    }

    public void Remove(Emprestimo emprestimo)
    {
        _db.Emprestimos.Remove(emprestimo);
        _db.SaveChanges();
    }
}