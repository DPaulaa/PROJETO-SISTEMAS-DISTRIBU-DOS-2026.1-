// Models/Livro.cs
// Representa um livro dentro da aplicação. Só guarda dados — sem lógica.
namespace BibliotecaRosa.Models;

public class Livro
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
