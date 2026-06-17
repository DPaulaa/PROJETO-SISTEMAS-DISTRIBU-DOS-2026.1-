using BibliotecaRosa.Exceptions;
using BibliotecaRosa.Models;
using BibliotecaRosa.Models.DTOs;
using BibliotecaRosa.Repositories.Interfaces;
using BibliotecaRosa.Services.Interfaces;

namespace BibliotecaRosa.Services;

public class EmprestimoService : IEmprestimoService
{
    private readonly IEmprestimoRepository _emprestimoRepository;
    private readonly ILivroRepository _livroRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ILogger<EmprestimoService> _logger;

    public EmprestimoService(
        IEmprestimoRepository emprestimoRepository,
        ILivroRepository livroRepository,
        IUsuarioRepository usuarioRepository,
        ILogger<EmprestimoService> logger)
    {
        _emprestimoRepository = emprestimoRepository;
        _livroRepository = livroRepository;
        _usuarioRepository = usuarioRepository;
        _logger = logger;
    }

    public IEnumerable<EmprestimoResponse> GetAll()
    {
        var emprestimos = _emprestimoRepository.GetAll();
        return emprestimos.Select(MapearParaResponse);
    }

    public EmprestimoResponse GetById(int id)
    {
        var emprestimo = _emprestimoRepository.GetById(id)
            ?? throw new RecursoNaoEncontradoException($"Empréstimo {id} não encontrado.");
        return MapearParaResponse(emprestimo);
    }

    public void Delete(int id)
    {
        var emprestimo = _emprestimoRepository.GetById(id)
            ?? throw new RecursoNaoEncontradoException($"Empréstimo {id} não encontrado.");
        _emprestimoRepository.Remove(emprestimo);
        _logger.LogInformation("Empréstimo removido: Id={Id}", id);
    }

    public EmprestimoResponse Emprestar(EmprestimoRequest request)
    {
        // Validações
        var livro = _livroRepository.GetById(request.LivroId)
            ?? throw new RecursoNaoEncontradoException($"Livro {request.LivroId} não encontrado.");

        var usuario = _usuarioRepository.BuscarPorIdAsync(request.UsuarioId).GetAwaiter().GetResult()
            ?? throw new RecursoNaoEncontradoException($"Usuário {request.UsuarioId} não encontrado.");

        // Verifica se o livro está disponível
        if (livro.QuantidadeDisponivel <= 0)
            throw new RegraDeNegocioException($"Livro '{livro.Titulo}' não está disponível para empréstimo.");

        // Cria o empréstimo
        var emprestimo = new Emprestimo
        {
            LivroId = request.LivroId,
            UsuarioId = request.UsuarioId,
            DataEmprestimo = DateTime.UtcNow,
            DataDevolucaoPrevista = DateTime.UtcNow.AddDays(7) // 7 dias para devolução
        };

        // Decrementa o estoque do livro
        livro.QuantidadeDisponivel -= 1;
        _livroRepository.Update(livro);

        _emprestimoRepository.Add(emprestimo);
        _logger.LogInformation("Novo empréstimo criado: Livro={LivroId}, Usuario={UsuarioId}", request.LivroId, request.UsuarioId);

        return MapearParaResponse(emprestimo);
    }

    public EmprestimoResponse Devolver(int idEmprestimo, int usuarioLogadoId)
    {
        var emprestimo = _emprestimoRepository.GetById(idEmprestimo)
            ?? throw new RecursoNaoEncontradoException($"Empréstimo {idEmprestimo} não encontrado.");

        // Só o usuário que fez o empréstimo pode devolvê-lo por aqui.
        // Para devolver em nome de outro usuário, o Admin deve usar forcar-devolucao.
        if (emprestimo.UsuarioId != usuarioLogadoId)
            throw new RegraDeNegocioException("Você só pode devolver empréstimos feitos por você mesmo.");

        if (emprestimo.DataDevolucao.HasValue)
            throw new RegraDeNegocioException($"Empréstimo {idEmprestimo} já foi devolvido.");

        emprestimo.DataDevolucao = DateTime.UtcNow;
        _emprestimoRepository.Update(emprestimo);

        // Devolve o exemplar ao estoque
        var livro = _livroRepository.GetById(emprestimo.LivroId);
        if (livro is not null)
        {
            livro.QuantidadeDisponivel += 1;
            _livroRepository.Update(livro);
        }

        _logger.LogInformation("Devolução registrada: Empréstimo={Id}", idEmprestimo);

        return MapearParaResponse(emprestimo);
    }

    // ── Admin ─────────────────────────────────────────────────────────

    public IEnumerable<EmprestimoResponse> GetAllAdmin()
    {
        var emprestimos = _emprestimoRepository.GetAll();
        return emprestimos.Select(MapearParaResponse);
    }

    public IEnumerable<EmprestimoResponse> GetByUsuario(int usuarioId)
    {
        var emprestimos = _emprestimoRepository.GetByUsuarioId(usuarioId);
        return emprestimos.Select(MapearParaResponse);
    }

    public EmprestimoResponse ForcarDevolucao(int idEmprestimo)
    {
        var emprestimo = _emprestimoRepository.GetById(idEmprestimo)
            ?? throw new RecursoNaoEncontradoException($"Empréstimo {idEmprestimo} não encontrado.");

        if (emprestimo.DataDevolucao.HasValue)
            throw new RegraDeNegocioException($"Empréstimo {idEmprestimo} já foi devolvido.");

        emprestimo.DataDevolucao = DateTime.UtcNow;
        _emprestimoRepository.Update(emprestimo);

        // Devolve o exemplar ao estoque também na devolução forçada pelo Admin
        var livro = _livroRepository.GetById(emprestimo.LivroId);
        if (livro is not null)
        {
            livro.QuantidadeDisponivel += 1;
            _livroRepository.Update(livro);
        }

        _logger.LogWarning("Devolução FORÇADA pelo Admin: Empréstimo={Id}", idEmprestimo);

        return MapearParaResponse(emprestimo);
    }

    public IEnumerable<RelatorioLivroDto> GetRelatorioMaisEmprestados()
    {
        var emprestimos = _emprestimoRepository.GetAll();
        var relatorio = emprestimos
            .GroupBy(e => e.LivroId)
            .Select(g => new RelatorioLivroDto
            {
                LivroId = g.Key,
                Titulo = g.First().Livro?.Titulo ?? "Desconhecido",
                Autor = g.First().Livro?.Autor ?? "Desconhecido",
                TotalEmprestimos = g.Count()
            })
            .OrderByDescending(r => r.TotalEmprestimos)
            .ToList();

        return relatorio;
    }

    // ── Histórico ──────────────────────────────────────────────────────

    public IEnumerable<EmprestimoResponse> GetHistoricoDoUsuario(int usuarioId)
    {
        var emprestimos = _emprestimoRepository.GetByUsuarioId(usuarioId);
        return emprestimos.Select(MapearParaResponse);
    }

    public IEnumerable<EmprestimoResponse> GetMeusEmprestimosAtivos(int usuarioId)
    {
        var emprestimos = _emprestimoRepository.GetEmprestimosAtivosPorUsuario(usuarioId);
        return emprestimos.Select(MapearParaResponse);
    }

    // ── Mapeamento ─────────────────────────────────────────────────────

    private static EmprestimoResponse MapearParaResponse(Emprestimo e)
    {
        return new EmprestimoResponse
        {
            Id = e.Id,
            UsuarioId = e.UsuarioId,
            UsuarioNome = e.Usuario?.Nome ?? "Desconhecido",
            UsuarioRole = e.Usuario?.Role.ToString() ?? "Desconhecido",
            LivroId = e.LivroId,
            LivroTitulo = e.Livro?.Titulo ?? "Desconhecido",
            DataEmprestimo = e.DataEmprestimo,
            DataDevolucaoPrevista = e.DataDevolucaoPrevista,
            DataDevolucao = e.DataDevolucao
        };
    }
}