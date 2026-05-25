namespace BibliotecaRosa.Models;

public class Emprestimo
{
    public int Id { get; set; }
    public Livro Livro { get; set; } = new Livro();
    public Pessoa Pessoa { get; set; } = new Pessoa();
    public DateTime DataEmprestimo { get; set; } = DateTime.UtcNow;
    public DateTime? DataDevolucao { get; set; }
    public DateTime? DataDevolucaoPrevista {get; set; }
}