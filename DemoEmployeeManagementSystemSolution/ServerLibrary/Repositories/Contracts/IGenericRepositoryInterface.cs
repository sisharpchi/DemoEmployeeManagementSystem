using BaseLibrary.Responses;

namespace ServerLibrary.Repositories.Contracts;

public interface IGenericRepositoryInterface<T>
{
    Task<List<T>> GetAll();
    Task<T> GetById(int id);
    Task<GeneralRepsonse> Insert(T item);
    Task<GeneralRepsonse> Update(T item);
    Task<GeneralRepsonse> DeleteById(int id);
}
