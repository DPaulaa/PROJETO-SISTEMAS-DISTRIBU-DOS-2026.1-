// Services/LivroService.cs
//
// Aqui ficam as regras de negócio dos livros: validar ISBN duplicado, montar o objeto antes de salvar, registrar logs de operação, etc.
// Não sabe nada de HTTP — só processa dados e chama o repositório.
// Se amanhã precisarmos de um LivroServiceComCache, basta criar uma nova classe com o mesmo contrato (ILivroService) sem mexer aqui.
namespace BibliotecaRosa.Services;

using BibliotecaRosa.Exceptions;
using BibliotecaRosa.Models;
using BibliotecaRosa.Models.DTOs;
using BibliotecaRosa.Repositories.Interfaces;
using BibliotecaRosa.Services.Interfaces;

public class LivroService : ILivroService
{
    // O repositório é injetado — quem chama o serviço não precisa saber como os dados são guardados
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
            QuantidadeDisponivel = request.QuantidadeDisponivel,
            CreatedAt            = DateTime.UtcNow
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
        livro.QuantidadeDisponivel = request.QuantidadeDisponivel;

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

    // Converte o objeto interno (Livro) para o formato que será enviado ao cliente (LivroResponse).
    // Assim o cliente nunca vê campos internos que não são relevantes para ele.
    private static LivroResponse Mapear(Livro l) => new()
    {
        Id                   = l.Id,
        Titulo               = l.Titulo,
        Autor                = l.Autor,
        Isbn                 = l.Isbn,
        QuantidadeDisponivel = l.QuantidadeDisponivel,
        CreatedAt            = l.CreatedAt
    };
}