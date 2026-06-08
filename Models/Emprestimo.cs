namespace BibliotecaRosa.Models;

public class Emprestimo
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public int LivroId { get; set; }
    public Livro Livro { get; set; } = null!;

    public DateTime DataEmprestimo { get; set; } = DateTime.UtcNow;
    public DateTime? DataDevolucao { get; set; }
    public DateTime? DataDevolucaoPrevista { get; set; }
}
