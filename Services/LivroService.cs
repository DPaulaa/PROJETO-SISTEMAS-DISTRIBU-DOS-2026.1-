// Services/LivroService.cs
//
// CORREÇÃO BUG 5: Create() e Update() não mapeavam QuantidadeDisponivel do
// request para a entidade Livro — o campo sempre ficava em 0 no banco.
// Adicionado o mapeamento em ambos os métodos e no Mapear() de saída.

namespace BibliotecaRosa.Services;

using BibliotecaRosa.Exceptions;
using BibliotecaRosa.Models;
using BibliotecaRosa.Models.DTOs;
using BibliotecaRosa.Repositories.Interfaces;
using BibliotecaRosa.Services.Interfaces;

public class LivroService : ILivroService
{
    private readonly ILivroRepository _repo;
    private readonly ILogger<LivroService> _logger;

    public LivroService(ILivroRepository repo, ILogger<LivroService> logger)
    {
        _repo   = repo;
        _logger = logger;
    }

    public IEnumerable<LivroResponse> GetAll() =>
        _repo.GetAll().Select(Mapear);

    public LivroResponse GetById(int id)
    {
        var livro = _repo.GetById(id)
            ?? throw new RecursoNaoEncontradoException($"Livro {id} não encontrado.");
        return Mapear(livro);
    }

    public LivroResponse Create(LivroRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.Isbn))
        {
            var existente = _repo.GetByIsbn(request.Isbn.Trim());
            if (existente is not null)
                throw new RegraDeNegocioException($"Já existe um livro com ISBN {request.Isbn}.");
        }

        var livro = new Livro
        {
            Titulo               = request.Titulo.Trim(),
            Autor                = request.Autor.Trim(),
            Isbn                 = request.Isbn?.Trim() ?? "Sem ISBN",
            CreatedAt            = DateTime.UtcNow,
            QuantidadeDisponivel = request.QuantidadeDisponivel  // CORREÇÃO BUG 5
        };

        _repo.Add(livro);
        _logger.LogInformation("Livro criado: '{Titulo}' (Id={Id})", livro.Titulo, livro.Id);
        return Mapear(livro);
    }

    public LivroResponse Update(int id, LivroRequest request)
    {
        var livro = _repo.GetById(id)
            ?? throw new RecursoNaoEncontradoException($"Livro {id} não encontrado.");

        livro.Titulo               = request.Titulo.Trim();
        livro.Autor                = request.Autor.Trim();
        livro.Isbn                 = request.Isbn?.Trim() ?? livro.Isbn;
        livro.QuantidadeDisponivel = request.QuantidadeDisponivel;  // CORREÇÃO BUG 5

        _repo.Update(livro);
        _logger.LogInformation("Livro atualizado: Id={Id}", id);
        return Mapear(livro);
    }

    public void Delete(int id)
    {
        var livro = _repo.GetById(id)
            ?? throw new RecursoNaoEncontradoException($"Livro {id} não encontrado.");

        _repo.Remove(livro);
        _logger.LogInformation("Livro removido: Id={Id}", id);
    }

    private static LivroResponse Mapear(Livro l) => new()
    {
        Id                   = l.Id,
        Titulo               = l.Titulo,
        Autor                = l.Autor,
        Isbn                 = l.Isbn,
        CreatedAt            = l.CreatedAt,
        QuantidadeDisponivel = l.QuantidadeDisponivel  // CORREÇÃO BUG 5
    };
}
