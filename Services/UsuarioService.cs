using BibliotecaRosa.Exceptions;
using BibliotecaRosa.Models;
using BibliotecaRosa.Models.DTOs;
using BibliotecaRosa.Repositories.Interfaces;
using BibliotecaRosa.Services.Interfaces;

namespace BibliotecaRosa.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repository;

    public UsuarioService(IUsuarioRepository repository) => _repository = repository;

    public async Task<IEnumerable<UsuarioRespostaDto>> ObterTodosAsync()
    {
        var usuarios = await _repository.BuscarTodosAsync();
        return usuarios.Select(Mapear);
    }

    public async Task<UsuarioRespostaDto> ObterPorIdAsync(int id)
    {
        var usuario = await _repository.BuscarPorIdAsync(id)
            ?? throw new RecursoNaoEncontradoException($"Usuário {id} não encontrado.");
        return Mapear(usuario);
    }

    public async Task<UsuarioRespostaDto> CadastrarAsync(UsuarioCadastroDto dto)
    {
        var existente = await _repository.BuscarPorEmailAsync(dto.Email);
        if (existente != null)
            throw new ConflitoException("Este e-mail já está cadastrado.");

        string senhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

        var novoUsuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            SenhaHash = senhaHash,
            Role = dto.Perfil
        };

        await _repository.AdicionarAsync(novoUsuario);
        return Mapear(novoUsuario);
    }

    public async Task<UsuarioRespostaDto> AtualizarAsync(int id, UsuarioAtualizacaoDto dto)
    {
        var usuario = await _repository.BuscarPorIdAsync(id)
            ?? throw new RecursoNaoEncontradoException($"Usuário {id} não encontrado.");

        if (!string.IsNullOrWhiteSpace(dto.Nome))
            usuario.Nome = dto.Nome;

        if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email != usuario.Email)
        {
            var emailEmUso = await _repository.BuscarPorEmailAsync(dto.Email);
            if (emailEmUso != null)
                throw new ConflitoException("Este e-mail já está em uso por outro usuário.");
            usuario.Email = dto.Email;
        }

        if (!string.IsNullOrWhiteSpace(dto.Senha))
            usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

        await _repository.AtualizarAsync(usuario);
        return Mapear(usuario);
    }

    public async Task RemoverAsync(int id)
    {
        var usuario = await _repository.BuscarPorIdAsync(id)
            ?? throw new RecursoNaoEncontradoException($"Usuário {id} não encontrado.");
        await _repository.RemoverAsync(usuario);
    }

    private static UsuarioRespostaDto Mapear(Usuario u) =>
        new(u.Id, u.Nome, u.Email, u.Role);
}
