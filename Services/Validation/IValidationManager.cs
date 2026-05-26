namespace BibliotecaRosa.Services.Validation;

public interface IValidationManager<T>
{
    void Validate(T entity);
}