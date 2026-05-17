using System.ComponentModel.DataAnnotations;

namespace LivrariaRosa.Models.DTOs.Requests;

public class LivroRequest
{
    [Required(ErrorMessage = "O campo 'titulo' é obrigatório.")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "O titulo deve ter entre 1 e 200 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo 'autor' é obrigatório.")]
    [StringLength(150, MinimumLength = 1, ErrorMessage = "O autor deve ter entre 1 e 150 caracteres.")]
    public string Autor { get; set; } = string.Empty;

    [StringLength(20, ErrorMessage = "O ISBN deve ter no máximo 20 caracteres.")]
    public string? Isbn { get; set; }
}
