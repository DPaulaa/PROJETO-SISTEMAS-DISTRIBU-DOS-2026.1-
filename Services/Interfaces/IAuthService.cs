namespace BibliotecaRosa.Services.Interfaces;

public interface IAuthService
{
    string GerarToken(string email, string role);
}