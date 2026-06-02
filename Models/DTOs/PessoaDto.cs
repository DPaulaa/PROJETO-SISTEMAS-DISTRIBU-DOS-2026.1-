using System.ComponentModel.DataAnnotations;

namespace BibliotecaRosa.Models.DTOs;

public class PessoaRequest
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string Nome { get; set; } = string.Empty;
    [Required(ErrorMessage = "O tipo de pessoa é obrigatório.")]
    public TipoPessoa TipoPessoa { get; set; }

    [Required(ErrorMessage = "O CPF ou CNPJ é obrigatório.")]
    public string CpfCnpj { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
}

public class PessoaResponse
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public TipoPessoa TipoPessoa { get; set; }
    public string CpfCnpj { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
}
