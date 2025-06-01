using BaseLibrary.Responses;
using ClientLibrary.Helpers;
using ClientLibrary.Services.Contracts;
using System.Net.Http.Json;

namespace ClientLibrary.Services.Implementations;

public class GenericSeriveImplemantation<T>(GetHttpClient getHttpClient) : IGenericServiceInterface<T>
{
    public async Task<GeneralRepsonse> DeleteById(int id, string baseUrl)
    {
        var httpClient = await getHttpClient.GetPrivateHttpClinet();
        var response = await httpClient.DeleteAsync($"{baseUrl}/delete/{id}");
        var rusult = await response.Content.ReadFromJsonAsync<GeneralRepsonse>();
        return rusult!;
    }

    public async Task<List<T>> GetAll(string baseUrl)
    {
        var httpClient = await getHttpClient.GetPrivateHttpClinet();
        var result = await httpClient.GetFromJsonAsync<List<T>>($"{baseUrl}/all");
        return result!;
    }

    public async Task<T> GetById(int id, string baseUrl)
    {
        var httpClient = await getHttpClient.GetPrivateHttpClinet();
        var result = await httpClient.GetFromJsonAsync<T>($"{baseUrl}/single/{id}");
        return result!;
    }

    public async Task<GeneralRepsonse> Insert(T item, string baseUrl)
    {
        var httpClient = await getHttpClient.GetPrivateHttpClinet();
        var response = await httpClient.PostAsJsonAsync($"{baseUrl}/add", item);
        var result = await response.Content.ReadFromJsonAsync<GeneralRepsonse>();
        return result!;
    }

    public async Task<GeneralRepsonse> Update(T item, string baseUrl)
    {
        var httpClient = await getHttpClient.GetPrivateHttpClinet();
        var response = await httpClient.PutAsJsonAsync($"{baseUrl}/update", item);
        var result = await response.Content.ReadFromJsonAsync<GeneralRepsonse>();
        return result!;
    }
}
