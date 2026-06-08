namespace BibliotecaRosa.Repositories;

using BibliotecaRosa.Data;
using BibliotecaRosa.Models;
using BibliotecaRosa.Models.DTOs;
using BibliotecaRosa.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class EmprestimoRepository : IEmprestimoRepository
{
    private readonly AppDbContext _db;

    public EmprestimoRepository(AppDbContext db) => _db = db;

    public IEnumerable<Emprestimo> GetAll() =>
        _db.Emprestimos
           .Include(e => e.Livro)
           .Include(e => e.Usuario)
           .ToList();

    public Emprestimo? GetById(int id) =>
        _db.Emprestimos
           .Include(e => e.Livro)
           .Include(e => e.Usuario)
           .FirstOrDefault(e => e.Id == id);

    public List<Emprestimo> GetByUsuarioId(int usuarioId) =>
        _db.Emprestimos
           .Include(e => e.Livro)
           .Include(e => e.Usuario)
           .Where(e => e.UsuarioId == usuarioId)
           .ToList();

    public List<Emprestimo> GetByLivroAndUsuario(int livroId, int usuarioId) =>
        _db.Emprestimos
           .Include(e => e.Livro)
           .Include(e => e.Usuario)
           .Where(e => e.LivroId == livroId && e.UsuarioId == usuarioId)
           .ToList();

    public List<Emprestimo> GetAtivos() =>
        _db.Emprestimos
           .Include(e => e.Livro)
           .Include(e => e.Usuario)
           .Where(e => e.DataDevolucao == null)
           .ToList();

    public List<RelatorioLivroDto> GetRelatorioMaisEmprestados() =>
        _db.Emprestimos
           .Include(e => e.Livro)
           .GroupBy(e => new { e.LivroId, e.Livro.Titulo, e.Livro.Autor })
           .Select(g => new RelatorioLivroDto
           {
               LivroId = g.Key.LivroId,
               Titulo = g.Key.Titulo,
               Autor = g.Key.Autor,
               TotalEmprestimos = g.Count()
           })
           .OrderByDescending(r => r.TotalEmprestimos)
           .ToList();

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
}
