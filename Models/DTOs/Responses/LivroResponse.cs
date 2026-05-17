namespace LivrariaRosa.Models.DTOs.Responses;

public class LivroResponse
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
