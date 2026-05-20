// Services/Interfaces/ILivroService.cs
//
// Define o "contrato" das operações de livros disponíveis para os controllers.
// O controller não sabe como as coisas são feitas — só sabe o que pode pedir.
namespace BibliotecaRosa.Services.Interfaces;

using BibliotecaRosa.Models.DTOs;

public interface ILivroService
{
    IEnumerable<LivroResponse> GetAll();
    LivroResponse GetById(int id);
    LivroResponse Create(LivroRequest request);
    LivroResponse Update(int id, LivroRequest request);
    void Delete(int id);
}
