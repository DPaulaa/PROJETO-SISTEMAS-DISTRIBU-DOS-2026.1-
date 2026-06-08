namespace BibliotecaRosa.Services;

using BibliotecaRosa.Enums;
using BibliotecaRosa.Exceptions;
using BibliotecaRosa.Models;
using BibliotecaRosa.Models.DTOs;
using BibliotecaRosa.Repositories.Interfaces;
using BibliotecaRosa.Services.Interfaces;
using BibliotecaRosa.Services.Validation;

public class EmprestimoService : IEmprestimoService
{
    private readonly IEmprestimoRepository _repo;
    private readonly ILivroRepository _livroRepo;
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly ILogger<EmprestimoService> _logger;
    private readonly ValidacaoEmprestimo _validacao;

    public EmprestimoService(
        IEmprestimoRepository repo,
        ILivroRepository livroRepo,
        IUsuarioRepository usuarioRepo,
        ILogger<EmprestimoService> logger,
        ValidacaoEmprestimo validacao)
    {
        _repo = repo;
        _livroRepo = livroRepo;
        _usuarioRepo = usuarioRepo;
        _logger = logger;
        _validacao = validacao;
    }

    // ── Endpoints comuns ─────────────────────────────────────────────────────

    public IEnumerable<EmprestimoResponse> GetAll() =>
        _repo.GetAll().Select(Mapear);

    public EmprestimoResponse GetById(int id)
    {
        var emprestimo = _repo.GetById(id)
            ?? throw new RecursoNaoEncontradoException($"Empréstimo {id} não encontrado.");
        return Mapear(emprestimo);
    }

    public void Delete(int id)
    {
        var existente = _repo.GetById(id)
            ?? throw new RecursoNaoEncontradoException($"Empréstimo {id} não encontrado.");
        _repo.Remove(existente);
        _logger.LogInformation("Empréstimo deletado: Id={Id}", existente.Id);
    }

    public EmprestimoResponse Emprestar(EmprestimoRequest request)
    {
        var livro = _livroRepo.GetById(request.LivroId)
            ?? throw new RecursoNaoEncontradoException($"Livro {request.LivroId} não encontrado.");

        if (livro.QuantidadeDisponivel <= 0)
            throw new InvalidOperationException($"Livro '{livro.Titulo}' sem exemplares disponíveis.");

        var usuario = _usuarioRepo.BuscarPorIdAsync(request.UsuarioId).GetAwaiter().GetResult()
            ?? throw new RecursoNaoEncontradoException($"Usuário {request.UsuarioId} não encontrado.");

        var emprestimo = new Emprestimo
        {
            Livro = livro,
            Usuario = usuario,
            UsuarioId = usuario.Id,
            LivroId = livro.Id,
            DataEmprestimo = DateTime.UtcNow,
        };

        // Prazo: Aluno = 10 dias, Professor/Admin = 30 dias
        emprestimo.DataDevolucaoPrevista = usuario.Role == Role.Aluno
            ? DateTime.UtcNow.AddDays(10)
            : DateTime.UtcNow.AddDays(30);

        _validacao.Validate(emprestimo);

        livro.QuantidadeDisponivel--;
        _livroRepo.Update(livro);

        emprestimo = _repo.Add(emprestimo);
        // Recarrega com includes para o mapeamento
        emprestimo = _repo.GetById(emprestimo.Id)!;

        _logger.LogInformation("Empréstimo realizado: LivroId={LivroId}, UsuarioId={UsuarioId}, Id={Id}",
            emprestimo.LivroId, emprestimo.UsuarioId, emprestimo.Id);

        return Mapear(emprestimo);
    }

    public EmprestimoResponse Devolver(int idEmprestimo)
    {
        var emprestimo = _repo.GetById(idEmprestimo)
            ?? throw new RecursoNaoEncontradoException($"Empréstimo {idEmprestimo} não encontrado.");

        if (emprestimo.DataDevolucao != null)
            throw new InvalidOperationException($"Empréstimo {idEmprestimo} já foi devolvido em {emprestimo.DataDevolucao:dd/MM/yyyy}.");

        emprestimo.DataDevolucao = DateTime.UtcNow;

        var livro = emprestimo.Livro;
        livro.QuantidadeDisponivel++;
        _livroRepo.Update(livro);

        _repo.Update(emprestimo);
        _logger.LogInformation("Empréstimo devolvido: Id={Id}", emprestimo.Id);
        return Mapear(emprestimo);
    }

    // ── Admin ─────────────────────────────────────────────────────────────────

    public IEnumerable<EmprestimoResponse> GetAllAdmin() =>
        _repo.GetAll().Select(Mapear);

    public IEnumerable<EmprestimoResponse> GetByUsuario(int usuarioId) =>
        _repo.GetByUsuarioId(usuarioId).Select(Mapear);

    public EmprestimoResponse ForcarDevolucao(int idEmprestimo)
    {
        var emprestimo = _repo.GetById(idEmprestimo)
            ?? throw new RecursoNaoEncontradoException($"Empréstimo {idEmprestimo} não encontrado.");

        if (emprestimo.DataDevolucao != null)
            throw new InvalidOperationException($"Empréstimo {idEmprestimo} já foi devolvido.");

        emprestimo.DataDevolucao = DateTime.UtcNow;

        var livro = emprestimo.Livro;
        livro.QuantidadeDisponivel++;
        _livroRepo.Update(livro);

        _repo.Update(emprestimo);
        _logger.LogInformation("Devolução forçada pelo admin: Id={Id}", emprestimo.Id);
        return Mapear(emprestimo);
    }

    public IEnumerable<RelatorioLivroDto> GetRelatorioMaisEmprestados() =>
        _repo.GetRelatorioMaisEmprestados();

    // ── Histórico ─────────────────────────────────────────────────────────────

    public IEnumerable<EmprestimoResponse> GetHistoricoDoUsuario(int usuarioId) =>
        _repo.GetByUsuarioId(usuarioId).Select(Mapear);

    public IEnumerable<EmprestimoResponse> GetMeusEmprestimosAtivos(int usuarioId) =>
        _repo.GetByUsuarioId(usuarioId)
             .Where(e => e.DataDevolucao == null)
             .Select(Mapear);

    // ── Mapper ────────────────────────────────────────────────────────────────

    private static EmprestimoResponse Mapear(Emprestimo e) => new()
    {
        Id = e.Id,
        UsuarioId = e.UsuarioId,
        UsuarioNome = e.Usuario?.Nome ?? string.Empty,
        UsuarioRole = e.Usuario?.Role.ToString() ?? string.Empty,
        LivroId = e.LivroId,
        LivroTitulo = e.Livro?.Titulo ?? string.Empty,
        DataEmprestimo = e.DataEmprestimo,
        DataDevolucao = e.DataDevolucao,
        DataDevolucaoPrevista = e.DataDevolucaoPrevista
    };
}
