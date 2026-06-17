using BibliotecaRosa.Enums;

namespace BibliotecaRosa.Models.DTOs
{
    // DTO de entrada para criação/atualização
    public record UsuarioCadastroDto(string Nome, string Email, string Senha, Role Perfil);

    // DTO de saída para respostas da API (não mostra a senha)
   public record UsuarioRespostaDto(int Id, string Nome, string Email, Role Perfil, string SenhaHash);

   public record UsuarioAtualizacaoDto(string Nome, string Email, string? Senha, Role Perfil);
}