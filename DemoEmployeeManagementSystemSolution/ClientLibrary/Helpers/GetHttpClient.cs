using BaseLibrary.DTOs;

namespace ClientLibrary.Helpers;

public class GetHttpClient(IHttpClientFactory httpClientFactory, LocalStorageService localStorageService)
{
    private const string HeaderKey = "Authorization";

    public async Task<HttpClient> GetPrivateHttpClinet()
    {
        var clinet = httpClientFactory.CreateClient("SystemApiClient");
        var stringToken = await localStorageService.GetToken();
        if (string.IsNullOrEmpty(stringToken)) return clinet;

        var deserializeToken = Serializations.DeserializeJsonString<UserSession>(stringToken);
        if (deserializeToken is null) return clinet;

        clinet.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", deserializeToken.Token);
        return clinet;
    }

    public HttpClient GetPublicHttpClient()
    {
        var client = httpClientFactory.CreateClient("SystemApiClient");
        client.DefaultRequestHeaders.Remove(HeaderKey);
        return client;
    }
}
