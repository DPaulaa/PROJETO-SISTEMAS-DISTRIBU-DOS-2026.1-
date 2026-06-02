// Services/EmprestimoService.cs
//
// CORREÇÕES aplicadas:
//
// BUG 3 — Devolver() chamava Update(emprestimo.Id, emprestimo), mas Update() fazia
//   "existente = emprestimo" (reassign de variável local) — nunca persistia nada.
//   Corrigido: Devolver() agora chama _repo.Update(emprestimo) diretamente.
//
// BUG 4 — InvalidOperationException (livro indisponível, já devolvido, limite de
//   empréstimos) não era capturada pelo middleware, retornando 500. Corrigido no
//   middleware (ExceptionHandlingMiddleware.cs) — neste arquivo a exceção permanece
//   correta e semântica.

namespace BibliotecaRosa.Services;

using BibliotecaRosa.Exceptions;
using BibliotecaRosa.Models;
using BibliotecaRosa.Models.DTOs;
using BibliotecaRosa.Repositories.Interfaces;
using BibliotecaRosa.Services.Interfaces;
using BibliotecaRosa.Services.Validation;

public class EmprestimoService : IEmprestimoService
{
    private readonly IEmprestimoRepository _repo;
    private readonly ILivroRepository      _livroRepo;
    private readonly IPessoaRepository     _pessoaRepo;
    private readonly ILogger<EmprestimoService> _logger;
    private readonly ValidacaoEmprestimo    _validacao;

    public EmprestimoService(
        IEmprestimoRepository repo,
        ILivroRepository      livroRepo,
        IPessoaRepository     pessoaRepo,
        ILogger<EmprestimoService> logger,
        ValidacaoEmprestimo   validacao)
    {
        _repo       = repo;
        _livroRepo  = livroRepo;
        _pessoaRepo = pessoaRepo;
        _logger     = logger;
        _validacao  = validacao;
    }

    public IEnumerable<EmprestimoResponse> GetAll() =>
        _repo.GetAll().Select(Mapear);

    public EmprestimoResponse GetById(int id)
    {
        var emprestimo = _repo.GetById(id)
            ?? throw new RecursoNaoEncontradoException($"Empréstimo {id} não encontrado.");
        return Mapear(emprestimo);
    }

    public EmprestimoResponse Create(Emprestimo emprestimo)
    {
        var criado = _repo.Add(emprestimo);
        _logger.LogInformation("Empréstimo criado: LivroId={LivroId}, PessoaId={PessoaId}, Id={Id}",
            criado.Livro.Id, criado.Pessoa.Id, criado.Id);
        return Mapear(criado);
    }

    public EmprestimoResponse Update(int id, Emprestimo emprestimo)
    {
        var existente = _repo.GetById(id)
            ?? throw new RecursoNaoEncontradoException($"Empréstimo {id} não encontrado.");

        // Copia os campos editáveis para a entidade rastreada pelo EF Core
        existente.DataDevolucao        = emprestimo.DataDevolucao;
        existente.DataDevolucaoPrevista = emprestimo.DataDevolucaoPrevista;

        var atualizado = _repo.Update(existente);
        _logger.LogInformation("Empréstimo atualizado: Id={Id}", existente.Id);
        return Mapear(atualizado);
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
            throw new InvalidOperationException($"Livro '{livro.Titulo}' sem exemplares disponíveis para empréstimo.");

        var pessoa = _pessoaRepo.GetById(request.PessoaId)
            ?? throw new RecursoNaoEncontradoException($"Pessoa {request.PessoaId} não encontrada.");

        var emprestimo = new Emprestimo
        {
            Livro          = livro,
            Pessoa         = pessoa,
            DataEmprestimo = DateTime.UtcNow,
        };

        // Prazo de devolução: aluno tem 10 dias, demais têm 30
        emprestimo.DataDevolucaoPrevista = pessoa.TipoPessoa == TipoPessoa.Aluno
            ? DateTime.UtcNow.AddDays(10)
            : DateTime.UtcNow.AddDays(30);

        // Valida regras de negócio (limite de 5 empréstimos, livro já emprestado para mesma pessoa)
        _validacao.Validate(emprestimo);

        livro.QuantidadeDisponivel--;
        _livroRepo.Update(livro);

        emprestimo = _repo.Add(emprestimo);
        _logger.LogInformation("Empréstimo realizado: LivroId={LivroId}, PessoaId={PessoaId}, Id={Id}",
            emprestimo.Livro.Id, emprestimo.Pessoa.Id, emprestimo.Id);

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

        // CORREÇÃO BUG 3: persiste diretamente via repositório — não chama Update() do serviço
        // (que fazia "existente = emprestimo" — reassign de variável local, nunca persistia)
        _repo.Update(emprestimo);

        _logger.LogInformation("Empréstimo devolvido: Id={Id}", emprestimo.Id);
        return Mapear(emprestimo);
    }

    private static EmprestimoResponse Mapear(Emprestimo e) => new()
    {
        Id                   = e.Id,
        LivroId              = e.Livro.Id,
        PessoaId             = e.Pessoa.Id,
        DataEmprestimo       = e.DataEmprestimo,
        DataDevolucao        = e.DataDevolucao,
        DataDevolucaoPrevista = e.DataDevolucaoPrevista
    };
}
