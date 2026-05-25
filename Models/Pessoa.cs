namespace BibliotecaRosa.Models;
public class Pessoa
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string CpfCnpj { get; set; } = string.Empty;
    public TipoPessoa TipoPessoa { get; set; }
}