using BibliotecaRosa.Data;
using BibliotecaRosa.Models;
using BibliotecaRosa.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaRosa.Repositories;

public class SqlEmprestimoRepository : IEmprestimoRepository
{
    private readonly AppDbContext _context;

    public SqlEmprestimoRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Emprestimo> GetAll()
    {
        return _context.Emprestimos
            .Include(e => e.Livro)
            .Include(e => e.Usuario)
            .ToList();
    }

    public Emprestimo? GetById(int id)
    {
        return _context.Emprestimos
            .Include(e => e.Livro)
            .Include(e => e.Usuario)
            .FirstOrDefault(e => e.Id == id);
    }

    public IEnumerable<Emprestimo> GetByUsuarioId(int usuarioId)
    {
        return _context.Emprestimos
            .Include(e => e.Livro)
            .Include(e => e.Usuario)
            .Where(e => e.UsuarioId == usuarioId)
            .ToList();
    }

    public IEnumerable<Emprestimo> GetEmprestimosAtivosPorUsuario(int usuarioId)
    {
        return _context.Emprestimos
            .Include(e => e.Livro)
            .Include(e => e.Usuario)
            .Where(e => e.UsuarioId == usuarioId && e.DataDevolucao == null)
            .ToList();
    }

    public IEnumerable<Emprestimo> GetEmprestimosAtivosPorLivro(int livroId)
    {
        return _context.Emprestimos
            .Where(e => e.LivroId == livroId && e.DataDevolucao == null)
            .ToList();
    }

    public void Add(Emprestimo emprestimo)
    {
        _context.Emprestimos.Add(emprestimo);
        _context.SaveChanges();
    }

    public void Update(Emprestimo emprestimo)
    {
        _context.Emprestimos.Update(emprestimo);
        _context.SaveChanges();
    }

    public void Remove(Emprestimo emprestimo)
    {
        _context.Emprestimos.Remove(emprestimo);
        _context.SaveChanges();
    }
}