// Repositories/Interfaces/ILivroRepository.cs
//
// Define o "contrato" de acesso a dados de livros.
// O restante da aplicação depende apenas desta interface.
namespace BibliotecaRosa.Repositories.Interfaces;

using BibliotecaRosa.Models;

public interface ILivroRepository
{
    IEnumerable<Livro> GetAll();
    Livro? GetById(int id);
    Livro? GetByIsbn(string isbn);
    void Add(Livro livro);
    void Update(Livro livro);
    void Remove(Livro livro);
}
