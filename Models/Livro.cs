using System.ComponentModel.DataAnnotations;

namespace BibliotecaRosa.Models;

public class Livro
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Autor { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Isbn { get; set; } = string.Empty;

    [Required]
    [Range(0, int.MaxValue)]
    public int QuantidadeDisponivel { get; set; } = 1;  // Valor padrão 1

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}