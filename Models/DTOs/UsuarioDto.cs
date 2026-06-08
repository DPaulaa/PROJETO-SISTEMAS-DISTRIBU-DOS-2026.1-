using BibliotecaRosa.Enums;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaRosa.Models.DTOs;

public record UsuarioCadastroDto(string Nome, string Email, string Senha, Role Perfil);

public record UsuarioRespostaDto(int Id, string Nome, string Email, Role Perfil);

public class UsuarioAtualizacaoDto
{
    [MaxLength(100)]
    public string? Nome { get; set; }

    [MaxLength(150)]
    [EmailAddress]
    public string? Email { get; set; }

    [MinLength(6, ErrorMessage = "Senha deve ter ao menos 6 caracteres.")]
    public string? Senha { get; set; }
}
