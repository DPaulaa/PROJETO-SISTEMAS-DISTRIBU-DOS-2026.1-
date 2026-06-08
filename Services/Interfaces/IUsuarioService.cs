using BibliotecaRosa.Models.DTOs;

namespace BibliotecaRosa.Services.Interfaces;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioRespostaDto>> ObterTodosAsync();
    Task<UsuarioRespostaDto> ObterPorIdAsync(int id);
    Task<UsuarioRespostaDto> CadastrarAsync(UsuarioCadastroDto dto);
    Task<UsuarioRespostaDto> AtualizarAsync(int id, UsuarioAtualizacaoDto dto);
    Task RemoverAsync(int id);
}
