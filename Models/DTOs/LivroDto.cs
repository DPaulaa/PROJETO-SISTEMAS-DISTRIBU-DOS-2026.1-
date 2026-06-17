// Models/DTOs/LivroDto.cs
//
// DTOs (Data Transfer Objects) são os "formulários" da API:

using System.ComponentModel.DataAnnotations;

namespace BibliotecaRosa.Models.DTOs;

/// <summary>Dados que o cliente envia ao criar ou atualizar um livro.</summary>
public class LivroRequest
{
    [Required(ErrorMessage = "O campo 'titulo' é obrigatório.")]
    [MaxLength(200)]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo 'autor' é obrigatório.")]
    [MaxLength(150)]
    public string Autor { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Isbn { get; set; }
     [Range(0, int.MaxValue, ErrorMessage = "A quantidade disponível não pode ser negativa.")]
    public int QuantidadeDisponivel { get; set; } = 1;
}

/// <summary>Dados que a API retorna ao cliente — nunca a entidade diretamente.</summary>
public class LivroResponse
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public int QuantidadeDisponivel { get; set; }
    public DateTime CreatedAt { get; set; }
}
