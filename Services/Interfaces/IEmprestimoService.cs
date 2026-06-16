namespace BibliotecaRosa.Services.Interfaces;

using BibliotecaRosa.Models;
using BibliotecaRosa.Models.DTOs;

public interface IEmprestimoService
{
    IEnumerable<EmprestimoResponse> GetAll();
    EmprestimoResponse GetById(int id);
    void Delete(int id);
    EmprestimoResponse Emprestar(EmprestimoRequest request);
    EmprestimoResponse Devolver(int idEmprestimo);

    // Admin
    IEnumerable<EmprestimoResponse> GetAllAdmin();
    IEnumerable<EmprestimoResponse> GetByUsuario(int usuarioId);
    EmprestimoResponse ForcarDevolucao(int idEmprestimo);
    IEnumerable<RelatorioLivroDto> GetRelatorioMaisEmprestados();

    // Histórico
    IEnumerable<EmprestimoResponse> GetHistoricoDoUsuario(int usuarioId);
    IEnumerable<EmprestimoResponse> GetMeusEmprestimosAtivos(int usuarioId);
}