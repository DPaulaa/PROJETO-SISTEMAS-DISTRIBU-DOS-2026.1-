using BibliotecaRosa.Models.DTOs;

namespace BibliotecaRosa.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioRespostaDto>> ObterTodosAsync();
        Task<UsuarioRespostaDto> CadastrarAsync(UsuarioCadastroDto dto);
    }
}