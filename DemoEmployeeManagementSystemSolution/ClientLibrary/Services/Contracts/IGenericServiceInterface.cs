using BaseLibrary.Responses;

namespace ClientLibrary.Services.Contracts;

public interface IGenericServiceInterface<T>
{
    Task<List<T>> GetAll(string baseUrl);
    Task<T> GetById(int id, string baseUrl);
    Task<GeneralRepsonse> Insert(T item, string baseUrl);
    Task<GeneralRepsonse> Update(T item, string baseUrl);
    Task<GeneralRepsonse> DeleteById(int id, string baseUrl);
}
