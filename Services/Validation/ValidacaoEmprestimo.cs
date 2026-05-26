namespace BibliotecaRosa.Services.Validation;

using BibliotecaRosa.Models;
using BibliotecaRosa.Repositories.Interfaces;

public class ValidacaoEmprestimo : IValidationManager<Emprestimo>
{
    private readonly IEmprestimoRepository _emprestimoRepo;

    public ValidacaoEmprestimo(IEmprestimoRepository emprestimoRepo)
    {
        _emprestimoRepo = emprestimoRepo;
    }
    public void Validate(Emprestimo emprestimo)
    {
        if (emprestimo.Livro == null)
            throw new ArgumentException("O livro é obrigatório para um empréstimo.");

        if (emprestimo.Pessoa == null)
            throw new ArgumentException("A pessoa é obrigatória para um empréstimo.");

        if (emprestimo.DataDevolucao != null && emprestimo.DataDevolucao < emprestimo.DataEmprestimo)
            throw new ArgumentException("A data de devolução não pode ser anterior à data de empréstimo.");

        List<Emprestimo> emprestimosAtivos = _emprestimoRepo.GetByPessoaId(emprestimo.Pessoa.Id)
            .Where(e => e.DataDevolucao == null).ToList();

        if (emprestimosAtivos.Count > 5)
            throw new InvalidOperationException("A pessoa já possui 5 empréstimos ativos. Devolva um livro para realizar um novo empréstimo.");

        List<Emprestimo> hasLivroEmprestado = _emprestimoRepo.GetByLivroAndPessoa(emprestimo.Livro.Id, emprestimo.Pessoa.Id)
            .Where(e => e.DataDevolucao == null).ToList();

        if (hasLivroEmprestado.Count > 0)
            throw new InvalidOperationException("O livro já está emprestado para esta pessoa.");
    }
}