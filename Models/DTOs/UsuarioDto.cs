using System.ComponentModel.DataAnnotations;
using BibliotecaRosa.Enums;

namespace BibliotecaRosa.Models.DTOs
{
    // DTO de entrada para criação/atualização
    public record UsuarioCadastroDto(
        [Required(ErrorMessage = "O campo 'nome' é obrigatório.")]
        string Nome,

        [Required(ErrorMessage = "O campo 'email' é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido, no formato nome@dominio.com.")]
        string Email,

        [Required(ErrorMessage = "O campo 'senha' é obrigatório.")]
        string Senha,

        Role Perfil
    );

    // DTO de saída para respostas da API (não mostra a senha)
    public record UsuarioRespostaDto(int Id, string Nome, string Email, Role Perfil, string SenhaHash);

    public record UsuarioAtualizacaoDto(
        [Required(ErrorMessage = "O campo 'nome' é obrigatório.")]
        string Nome,

        [Required(ErrorMessage = "O campo 'email' é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido, no formato nome@dominio.com.")]
        string Email,

        string? Senha,

        Role Perfil
    );
}