using BibliotecaRosa.Data;
using BibliotecaRosa.Models;
using BibliotecaRosa.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaRosa.Repositories;

public class SqlUsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public SqlUsuarioRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Usuario>> BuscarTodosAsync() =>
        await _context.Usuarios.ToListAsync();

    public async Task<Usuario?> BuscarPorIdAsync(int id) =>
        await _context.Usuarios.FindAsync(id);

    public async Task<Usuario?> BuscarPorEmailAsync(string email) =>
        await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

    public async Task AdicionarAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Usuario usuario)
    {
        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
    }
}
