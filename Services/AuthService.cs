namespace BibliotecaRosa.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BibliotecaRosa.Exceptions;
using BibliotecaRosa.Models.DTOs;
using BibliotecaRosa.Repositories.Interfaces;
using BibliotecaRosa.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUsuarioRepository usuarioRepo, IConfiguration config, ILogger<AuthService> logger)
    {
        _usuarioRepo = usuarioRepo;
        _config = config;
        _logger = logger;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var usuario = await _usuarioRepo.BuscarPorEmailAsync(request.Email)
            ?? throw new RecursoNaoEncontradoException("E-mail ou senha inválidos.");

        bool senhaValida = BCrypt.Net.BCrypt.Verify(request.Senha, usuario.SenhaHash);
        if (!senhaValida)
            throw new RecursoNaoEncontradoException("E-mail ou senha inválidos.");

        var chaveJwt = _config["Jwt:Key"]
            ?? throw new InvalidOperationException("Jwt:Key não configurada.");
        var issuer = _config["Jwt:Issuer"] ?? "BibliotecaRosa";
        var audience = _config["Jwt:Audience"] ?? "BibliotecaRosaClientes";

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveJwt));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiracao = DateTime.UtcNow.AddHours(8);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Role, usuario.Role.ToString()),
            new Claim("role_id", ((int)usuario.Role).ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiracao,
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        _logger.LogInformation("Login realizado: UsuarioId={Id}, Role={Role}", usuario.Id, usuario.Role);

        return new LoginResponse
        {
            Token = tokenString,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Role = usuario.Role.ToString(),
            Expiracao = expiracao
        };
    }
}
