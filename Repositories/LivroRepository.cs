using Microsoft.EntityFrameworkCore;
using LivrariaRosa.Data;
using LivrariaRosa.Models.Entities;
using LivrariaRosa.Repositories.Interfaces;

namespace LivrariaRosa.Repositories;

public class LivroRepository : ILivroRepository
{
    private readonly AppDbContext _db;

    public LivroRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Livro>> ListarTodosAsync(int pagina, int tamanhoPagina)
    {
        return await _db.Livros
            .OrderBy(l => l.Id)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync();
    }

    public async Task<int> ContarTotalAsync()
    {
        return await _db.Livros.CountAsync();
    }

    public async Task<Livro?> BuscarPorIdAsync(int id)
    {
        return await _db.Livros.FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task AdicionarAsync(Livro livro)
    {
        _db.Livros.Add(livro);
        await _db.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Livro livro)
    {
        _db.Livros.Update(livro);
        await _db.SaveChangesAsync();
    }

    // Soft delete: marca como inativo em vez de remover fisicamente
    public async Task RemoverAsync(Livro livro)
    {
        livro.Ativo = false;
        livro.ExcluidoEm = DateTime.UtcNow;
        _db.Livros.Update(livro);
        await _db.SaveChangesAsync();
    }
}
