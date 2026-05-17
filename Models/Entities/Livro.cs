namespace LivrariaRosa.Models.Entities;

public class Livro
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool Ativo { get; set; } = true;
    public DateTime? ExcluidoEm { get; set; }
}
