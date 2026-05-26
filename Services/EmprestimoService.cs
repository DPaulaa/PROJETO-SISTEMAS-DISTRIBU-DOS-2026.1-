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
  private readonly ILivroRepository _livroRepo;
  private readonly IPessoaRepository _pessoaRepo;
  private readonly ILogger<EmprestimoService> _logger;
  private readonly ValidacaoEmprestimo _validacao;

  public EmprestimoService(IEmprestimoRepository repo, ILivroRepository livroRepo, IPessoaRepository pessoaRepo, ILogger<EmprestimoService> logger, ValidacaoEmprestimo validacao)
  {
    _repo = repo;
    _livroRepo = livroRepo;
    _pessoaRepo = pessoaRepo;
    _logger = logger;
    _validacao = validacao;
  }

  public IEnumerable<EmprestimoResponse> GetAll() =>
    _repo.GetAll().Select(emprestimoMapper);

  public EmprestimoResponse GetById(int id)
  {
    var emprestimo = _repo.GetById(id)
      ?? throw new RecursoNaoEncontradoException($"Empréstimo {id} não encontrado.");
    return emprestimoMapper(emprestimo);
  }

  public EmprestimoResponse Create(Emprestimo emprestimo)
  {
    var createdEmprestimo = _repo.Add(emprestimo);
    _logger.LogInformation("Empréstimo criado: LivroId={LivroId}, PessoaId={PessoaId}, Id={Id}", createdEmprestimo.Livro.Id, createdEmprestimo.Pessoa.Id, createdEmprestimo.Id);
    return emprestimoMapper(createdEmprestimo);
  }

  public EmprestimoResponse Update(int id, Emprestimo emprestimo)
  {
    var existente = _repo.GetById(id)
      ?? throw new RecursoNaoEncontradoException($"Empréstimo {id} não encontrado.");

    existente = emprestimo;

    var updatedEmprestimo = _repo.Update(existente);
    _logger.LogInformation("Empréstimo atualizado: Id={Id}", existente.Id);
    return emprestimoMapper(updatedEmprestimo);
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
      throw new InvalidOperationException($"Livro {request.LivroId} indisponível para empréstimo.");

    var pessoa = _pessoaRepo.GetById(request.PessoaId)
      ?? throw new RecursoNaoEncontradoException($"Pessoa {request.PessoaId} não encontrada.");

    _validacao.Validate(new Emprestimo { Livro = livro, Pessoa = pessoa, DataEmprestimo = DateTime.Now });

    var dataEmprestimo = pessoa.TipoPessoa == TipoPessoa.Aluno ? DateTime.Now.AddDays(10) : DateTime.Now.AddDays(30);

    var emprestimo = new Emprestimo
    {
      Livro = livro,
      Pessoa = pessoa,
      DataEmprestimo = DateTime.Now,
      DataDevolucaoPrevista = dataEmprestimo
    };

    livro.QuantidadeDisponivel--;
    _livroRepo.Update(livro);
    emprestimo = _repo.Add(emprestimo);
    _logger.LogInformation("Empréstimo realizado: LivroId={LivroId}, PessoaId={PessoaId}, Id={Id}", emprestimo.Livro.Id, emprestimo.Pessoa.Id, emprestimo.Id);

    return emprestimoMapper(emprestimo);
  }

  public EmprestimoResponse Devolver(int idEmprestimo)
  {
      var emprestimo = _repo.GetById(idEmprestimo)
          ?? throw new RecursoNaoEncontradoException($"Empréstimo {idEmprestimo} não encontrado.");

    if (emprestimo.DataDevolucao != null)
      throw new InvalidOperationException($"Empréstimo {idEmprestimo} já foi devolvido.");

    emprestimo.DataDevolucao = DateTime.Now;
    var livro = emprestimo.Livro;
    livro.QuantidadeDisponivel++;
    _livroRepo.Update(livro);
    Update(emprestimo.Id, emprestimo);
    _logger.LogInformation("Empréstimo devolvido: Id={Id}", emprestimo.Id);

    return emprestimoMapper(emprestimo);
  }

  private static EmprestimoResponse emprestimoMapper(Emprestimo e) =>
    new()
    {
      Id = e.Id,
      LivroId = e.Livro.Id,
      PessoaId = e.Pessoa.Id,
      DataEmprestimo = e.DataEmprestimo,
      DataDevolucao = e.DataDevolucao,
      DataDevolucaoPrevista = e.DataDevolucaoPrevista
    };
}