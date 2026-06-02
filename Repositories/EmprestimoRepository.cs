// Repositories/EmprestimoRepository.cs
//
// CORREÇÃO: todos os métodos de consulta estavam sem .Include(e => e.Livro) e
// .Include(e => e.Pessoa). Isso fazia com que emprestimo.Livro e emprestimo.Pessoa
// fossem null em runtime, causando NullReferenceException no mapper do serviço.
// Todos os métodos agora carregam as navegações obrigatórias.

namespace BibliotecaRosa.Repositories;

using BibliotecaRosa.Data;
using BibliotecaRosa.Models;
using BibliotecaRosa.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class EmprestimoRepository : IEmprestimoRepository
{
    private readonly AppDbContext _db;

    public EmprestimoRepository(AppDbContext db) => _db = db;

    public IEnumerable<Emprestimo> GetAll() =>
        _db.Emprestimos
           .Include(e => e.Livro)
           .Include(e => e.Pessoa)
           .ToList();

    public Emprestimo? GetById(int id) =>
        _db.Emprestimos
           .Include(e => e.Livro)
           .Include(e => e.Pessoa)
           .FirstOrDefault(e => e.Id == id);

    public List<Emprestimo> GetByPessoaId(int pessoaId) =>
        _db.Emprestimos
           .Include(e => e.Livro)
           .Include(e => e.Pessoa)
           .Where(e => e.Pessoa.Id == pessoaId)
           .ToList();

    public List<Emprestimo> GetByLivroAndPessoa(int livroId, int pessoaId) =>
        _db.Emprestimos
           .Include(e => e.Livro)
           .Include(e => e.Pessoa)
           .Where(e => e.Livro.Id == livroId && e.Pessoa.Id == pessoaId)
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
