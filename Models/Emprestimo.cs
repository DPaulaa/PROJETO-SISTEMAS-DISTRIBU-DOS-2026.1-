using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaRosa.Models;

public class Emprestimo
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("Livro")]
    public int LivroId { get; set; }
    public virtual Livro? Livro { get; set; }

    [Required]
    [ForeignKey("Usuario")]
    public int UsuarioId { get; set; }
    public virtual Usuario? Usuario { get; set; }

    [Required]
    public DateTime DataEmprestimo { get; set; } = DateTime.UtcNow;

    public DateTime? DataDevolucaoPrevista { get; set; }

    public DateTime? DataDevolucao { get; set; }
}