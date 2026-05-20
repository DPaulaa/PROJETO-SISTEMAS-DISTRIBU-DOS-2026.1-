// Services/Interfaces/IAuthService.cs
//
// Contrato de autenticação. Quem quiser verificar um token usa esta interface, sem precisar saber como a verificação é feita por baixo dos panos.
namespace BibliotecaRosa.Services.Interfaces;

public interface IAuthService
{
    bool IsAuthorized(string? authHeader);
}
