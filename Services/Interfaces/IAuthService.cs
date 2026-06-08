namespace BibliotecaRosa.Services.Interfaces;

using BibliotecaRosa.Models.DTOs;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
}
