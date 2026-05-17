using LivrariaRosa.Models.DTOs.Requests;
using LivrariaRosa.Models.DTOs.Responses;
using LivrariaRosa.Models.Entities;
using LivrariaRosa.Repositories.Interfaces;
using LivrariaRosa.Services.Interfaces;

namespace LivrariaRosa.Services;

public class LivroService : ILivroService
{
    private readonly ILivroRepository _repository;
    private readonly ILogger<LivroService> _logger;

    public LivroService(ILivroRepository repository, ILogger<LivroService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<object> ListarTodosAsync(int pagina, int tamanhoPagina)
    {
        _logger.LogInformation("Listando livros — página {Pagina}, tamanho {Tamanho}", pagina, tamanhoPagina);

        var livros  = await _repository.ListarTodosAsync(pagina, tamanhoPagina);
        var total   = await _repository.ContarTotalAsync();
        var totalPaginas = (int)Math.Ceiling((double)total / tamanhoPagina);

        return new
        {
            Dados        = livros.Select(MapToResponse),
            Pagina       = pagina,
            TamanhoPagina = tamanhoPagina,
            Total        = total,
            TotalPaginas = totalPaginas
        };
    }

    public async Task<LivroResponse?> BuscarPorIdAsync(int id)
    {
        _logger.LogInformation("Buscando livro com Id={Id}", id);
        var livro = await _repository.BuscarPorIdAsync(id);
        return livro is null ? null : MapToResponse(livro);
    }

    public async Task<LivroResponse> CriarAsync(LivroRequest request)
    {
        _logger.LogInformation("Criando livro: {Titulo}", request.Titulo);

        var livro = new Livro
        {
            Titulo    = request.Titulo.Trim(),
            Autor     = request.Autor.Trim(),
            Isbn      = request.Isbn?.Trim() ?? "Sem ISBN",
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AdicionarAsync(livro);
        _logger.LogInformation("Livro criado com Id={Id}", livro.Id);
        return MapToResponse(livro);
    }

    public async Task<LivroResponse?> AtualizarAsync(int id, LivroRequest request)
    {
        _logger.LogInformation("Atualizando livro Id={Id}", id);

        var livro = await _repository.BuscarPorIdAsync(id);
        if (livro is null) return null;

        livro.Titulo = request.Titulo.Trim();
        livro.Autor  = request.Autor.Trim();
        if (request.Isbn is not null)
            livro.Isbn = request.Isbn.Trim();

        await _repository.AtualizarAsync(livro);
        return MapToResponse(livro);
    }

    public async Task<bool> RemoverAsync(int id)
    {
        _logger.LogInformation("Removendo livro Id={Id}", id);

        var livro = await _repository.BuscarPorIdAsync(id);
        if (livro is null) return false;

        await _repository.RemoverAsync(livro);
        _logger.LogInformation("Livro Id={Id} removido (soft delete)", id);
        return true;
    }

    // Mapeamento privado: entidade → DTO de resposta (sem expor campos sensíveis)
    private static LivroResponse MapToResponse(Livro livro) => new()
    {
        Id        = livro.Id,
        Titulo    = livro.Titulo,
        Autor     = livro.Autor,
        Isbn      = livro.Isbn,
        CreatedAt = livro.CreatedAt
    };
}
