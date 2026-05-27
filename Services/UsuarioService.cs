using BibliotecaRosa.Exceptions;
using BibliotecaRosa.Models;
using BibliotecaRosa.Models.DTOs;
using BibliotecaRosa.Repositories.Interfaces;
using BibliotecaRosa.Services.Interfaces;

namespace BibliotecaRosa.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UsuarioRespostaDto>> ObterTodosAsync()
        {
            var usuarios = await _repository.BuscarTodosAsync();
            // Mapeia a entidade do banco para o DTO de saída
            return usuarios.Select(u => new UsuarioRespostaDto(u.Id, u.Nome, u.Email, u.Role));
        }

        public async Task<UsuarioRespostaDto> CadastrarAsync(UsuarioCadastroDto dto)
        {
            // Regra de negócio: Não permitir e-mails duplicados
            var usuarioExistente = await _repository.BuscarPorEmailAsync(dto.Email);
            if (usuarioExistente != null)
            {
                throw new ConflitoException("Este e-mail já está cadastrado.");
            }

            // Criptografia da senha usando BCrypt
            string senhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

            var novoUsuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = senhaHash,
                Role = dto.Perfil
            };

            await _repository.AdicionarAsync(novoUsuario);

            return new UsuarioRespostaDto(novoUsuario.Id, novoUsuario.Nome, novoUsuario.Email, novoUsuario.Role);
        }
    }
}