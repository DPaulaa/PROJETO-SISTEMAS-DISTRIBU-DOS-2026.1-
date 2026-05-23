// Repositories/SqlLivroRepository.cs
//
// Repositório que lê e grava no SQL Server do Azure via EF Core.
// Implementa o mesmo contrato (ILivroRepository) do repositório em memória —
// o restante da aplicação não precisa saber qual está sendo usado.
using BibliotecaRosa.Data;
using BibliotecaRosa.Models;
using BibliotecaRosa.Repositories.Interfaces;

namespace BibliotecaRosa.Repositories;

public class SqlLivroRepository : ILivroRepository
{
    private readonly AppDbContext _db;

    public SqlLivroRepository(AppDbContext db) => _db = db;

    public IEnumerable<Livro> GetAll() =>
        _db.Livros.ToList();

    public Livro? GetById(int id) =>
        _db.Livros.Find(id);

    public Livro? GetByIsbn(string isbn) =>
        _db.Livros.FirstOrDefault(l => l.Isbn == isbn);

    public void Add(Livro livro)
    {
        _db.Livros.Add(livro);
        _db.SaveChanges();
    }

    public void Update(Livro livro)
    {
        _db.Livros.Update(livro);
        _db.SaveChanges();
    }

    public void Remove(Livro livro)
    {
        _db.Livros.Remove(livro);
        _db.SaveChanges();
    }
}
