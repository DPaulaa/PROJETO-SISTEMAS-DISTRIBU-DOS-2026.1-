namespace BibliotecaRosa.Services.Validation;

using BibliotecaRosa.Enums;
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

        if (emprestimo.Usuario == null)
            throw new ArgumentException("O usuário é obrigatório para um empréstimo.");

        if (emprestimo.DataDevolucao != null && emprestimo.DataDevolucao < emprestimo.DataEmprestimo)
            throw new ArgumentException("A data de devolução não pode ser anterior à data de empréstimo.");

        // Limite de empréstimos por perfil: Aluno = 3, Professor = 5, Admin = sem limite
        var emprestimosAtivos = _emprestimoRepo.GetByUsuarioId(emprestimo.Usuario.Id)
            .Where(e => e.DataDevolucao == null).ToList();

        int limite = emprestimo.Usuario.Role switch
        {
            Role.Aluno => 3,
            Role.Professor => 5,
            Role.Administrador => int.MaxValue,
            _ => 3
        };

        if (emprestimosAtivos.Count >= limite)
            throw new InvalidOperationException(
                $"Usuário já atingiu o limite de {limite} empréstimos ativos para o perfil {emprestimo.Usuario.Role}.");

        // Não pode pegar o mesmo livro duas vezes ao mesmo tempo
        var jaTemLivro = _emprestimoRepo.GetByLivroAndUsuario(emprestimo.Livro.Id, emprestimo.Usuario.Id)
            .Any(e => e.DataDevolucao == null);

        if (jaTemLivro)
            throw new InvalidOperationException("O usuário já está com este livro emprestado.");
    }
}
