using System.ComponentModel.DataAnnotations;

namespace BibliotecaRosa.Models.DTOs;

public class EmprestimoRequest
{
    public int Id { get; set; }

    [Required(ErrorMessage = "É necessário selecionar uma pessoa.")]
    public int PessoaId { get; set; }

    [Required(ErrorMessage = "É necessário selecionar um livro.")]
    public int LivroId { get; set; }

    [Required(ErrorMessage = "Data empréstimo é obrigatória.")]
    public DateTime DataEmprestimo { get; set; }

    public DateTime? DataDevolucao { get; set; }

    public DateTime? DataDevolucaoPrevista { get; set; } = DateTime.Now.AddYears(999);
}

public class EmprestimoResponse
{
    public int Id { get; set; }
    public int PessoaId { get; set; }
    public int LivroId { get; set; }
    public DateTime DataEmprestimo { get; set; }
    public DateTime? DataDevolucao { get; set; }
    public DateTime? DataDevolucaoPrevista { get; set; }
}