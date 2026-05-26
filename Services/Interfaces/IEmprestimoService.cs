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
    EmprestimoResponse Emprestar(EmprestimoRequest emprestimo);
    EmprestimoResponse Devolver(int idEmprestimo);
}
