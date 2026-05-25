namespace BibliotecaRosa.Services;

using BibliotecaRosa.Exceptions;
using BibliotecaRosa.Models;
using BibliotecaRosa.Models.DTOs;
using BibliotecaRosa.Repositories.Interfaces;
using BibliotecaRosa.Services.Interfaces;

public class EmprestimoService : IEmprestimoService
{
  private readonly IEmprestimoRepository _repo;
  private readonly ILivroRepository _livroRepo;
  private readonly ILogger<EmprestimoService> _logger;

  public EmprestimoService(IEmprestimoRepository repo, ILivroRepository livroRepo, ILogger<EmprestimoService> logger)
  {
    _repo = repo;
    _livroRepo = livroRepo;
    _logger = logger;
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
    _repo.Add(emprestimo);
    _logger.LogInformation("Empréstimo criado: LivroId={LivroId}, PessoaId={PessoaId}, Id={Id}", emprestimo.Livro.Id, emprestimo.Pessoa.Id, emprestimo.Id);
    return emprestimoMapper(emprestimo);
  }

  public EmprestimoResponse Update(int id, Emprestimo emprestimo)
  {
    var existente = _repo.GetById(id)
      ?? throw new RecursoNaoEncontradoException($"Empréstimo {id} não encontrado.");

    existente = emprestimo;

    _repo.Update(existente);
    _logger.LogInformation("Empréstimo atualizado: Id={Id}", existente.Id);
    return emprestimoMapper(existente);
  }

  public void Delete(int id)
  {
    var existente = _repo.GetById(id)
      ?? throw new RecursoNaoEncontradoException($"Empréstimo {id} não encontrado.");

    _repo.Remove(existente);
    _logger.LogInformation("Empréstimo deletado: Id={Id}", existente.Id);
  }

  // public void Emprestar(EmprestimoRequest request)
  // {

  // }

  // public void Devolver(EmprestimoRequest request)
  // {

  // }

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