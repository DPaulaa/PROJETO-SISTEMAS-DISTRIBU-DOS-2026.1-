namespace BibliotecaRosa.Services.Interfaces;

using BibliotecaRosa.Models;
using BibliotecaRosa.Models.DTOs;

public interface IEmprestimoService
{
    IEnumerable<EmprestimoResponse> GetAll();
    EmprestimoResponse GetById(int id);
    EmprestimoResponse Create(Emprestimo emprestimo);
    EmprestimoResponse Update(int id, Emprestimo emprestimo);
    void Delete(int id);
    void Emprestar(EmprestimoRequest emprestimo);
    void Devolver(EmprestimoRequest emprestimo);
}
