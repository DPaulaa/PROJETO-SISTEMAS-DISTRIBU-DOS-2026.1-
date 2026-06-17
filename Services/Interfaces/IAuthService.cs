namespace BibliotecaRosa.Services.Interfaces;

public interface IAuthService
{
     string GerarToken(int id, string email, string role);
}