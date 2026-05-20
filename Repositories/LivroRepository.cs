// Repositories/LivroRepository.cs
//
// Camada de acesso a dados. Toda leitura e escrita de livros passa por aqui.

namespace BibliotecaRosa.Repositories;

using BibliotecaRosa.Models;
using BibliotecaRosa.Repositories.Interfaces;

public class LivroRepository : ILivroRepository
{
    // Dados de exemplo já carregados — em produção viriam do banco de dados
    private readonly List<Livro> _livros = new()
    {
        new Livro { Id = 1, Titulo = "O Senhor dos Anéis",   Autor = "J.R.R. Tolkien",           Isbn = "978-8533613379", CreatedAt = DateTime.UtcNow.AddDays(-180) },
        new Livro { Id = 2, Titulo = "1984",                 Autor = "George Orwell",             Isbn = "978-8535914849", CreatedAt = DateTime.UtcNow.AddDays(-120) },
        new Livro { Id = 3, Titulo = "Dom Casmurro",         Autor = "Machado de Assis",          Isbn = "978-8503011996", CreatedAt = DateTime.UtcNow.AddDays(-90)  },
        new Livro { Id = 4, Titulo = "O Pequeno Príncipe",   Autor = "Antoine de Saint-Exupéry", Isbn = "978-8595081512", CreatedAt = DateTime.UtcNow.AddDays(-60)  },
    };

    private int _nextId = 5;
    private readonly object _lock = new();

    public IEnumerable<Livro> GetAll()
    {
        lock (_lock) return _livros.ToList();
    }

    public Livro? GetById(int id)
    {
        lock (_lock) return _livros.FirstOrDefault(l => l.Id == id);
    }

    public Livro? GetByIsbn(string isbn)
    {
        lock (_lock) return _livros.FirstOrDefault(l => l.Isbn == isbn);
    }

    public void Add(Livro livro)
    {
        lock (_lock)
        {
            // Gera o próximo ID de forma segura mesmo com várias requisições simultâneas
            livro.Id = Interlocked.Increment(ref _nextId) - 1;
            _livros.Add(livro);
        }
    }

    public void Update(Livro livro)
    {
        lock (_lock)
        {
            var index = _livros.FindIndex(l => l.Id == livro.Id);
            if (index >= 0) _livros[index] = livro;
        }
    }

    public void Remove(Livro livro)
    {
        lock (_lock) _livros.Remove(livro);
    }
}
