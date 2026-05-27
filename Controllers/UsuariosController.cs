using BibliotecaRosa.Models.DTOs;
using BibliotecaRosa.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaRosa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var usuarios = await _usuarioService.ObterTodosAsync();
            return Ok(usuarios);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] UsuarioCadastroDto dto)
        {
            var novoUsuario = await _usuarioService.CadastrarAsync(dto);
            // Retorna 201 Created
            return CreatedAtAction(nameof(ObterTodos), new { id = novoUsuario.Id }, novoUsuario);
        }
    }
}