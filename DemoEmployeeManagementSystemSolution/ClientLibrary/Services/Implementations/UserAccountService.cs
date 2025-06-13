using BaseLibrary.DTOs;
using BaseLibrary.Entities;
using BaseLibrary.Responses;
using ClientLibrary.Helpers;
using ClientLibrary.Services.Contracts;
using System.Net.Http.Json;

namespace ClientLibrary.Services.Implementations;

public class UserAccountService(GetHttpClient getHttpClient) : IUserAccountService
{
    public const string AuthUrl = "api/authentication";

    public async Task<GeneralRepsonse> CreateAsync(Register user)
    {
        var httpClient = getHttpClient.GetPublicHttpClient();
        var result = await httpClient.PostAsJsonAsync($"{AuthUrl}/register", user);
        if (!result.IsSuccessStatusCode) return new GeneralRepsonse(false, "Error occurred");

        return await result.Content.ReadFromJsonAsync<GeneralRepsonse>()!;
    }

    public async Task<LoginResponse> SignInAsync(Login user)
    {
        var httpClient = getHttpClient.GetPublicHttpClient();
        var result = await httpClient.PostAsJsonAsync($"{AuthUrl}/login", user);
        if (!result.IsSuccessStatusCode) return new LoginResponse(false, "Error occurred");

        return await result.Content.ReadFromJsonAsync<LoginResponse>();
    }

    public async Task<LoginResponse> RefreshTokenAsync(RefreshToken token)
    {
        var httpClient = getHttpClient.GetPublicHttpClient();
        var result = await httpClient.PostAsJsonAsync($"{AuthUrl}/refresh-token", token);
        if (!result.IsSuccessStatusCode) return new LoginResponse(false, "Error occurred");

        return await result.Content.ReadFromJsonAsync<LoginResponse>()!;
    }


    public async Task<List<ManagerUser>> GetUsers()
    {
        var httpClient = await getHttpClient.GetPrivateHttpClinet();
        var result = await httpClient.GetFromJsonAsync<List<ManagerUser>>($"{AuthUrl}/users");
        return result!;
    }

    public async Task<GeneralRepsonse> UpdateUser(ManagerUser user)
    {
        var httpClient = getHttpClient.GetPublicHttpClient();
        var result = await httpClient.PutAsJsonAsync($"{AuthUrl}/update-user", user);
        if (!result.IsSuccessStatusCode) return new GeneralRepsonse(false, "Error occurred");

        return await result.Content.ReadFromJsonAsync<GeneralRepsonse>()!;
    }

    public async Task<List<SystemRole>> GetRoles()
    {
        var httpClient = await getHttpClient.GetPrivateHttpClinet();
        var result = await httpClient.GetFromJsonAsync<List<SystemRole>>($"{AuthUrl}/roles");
        return result!;
    }

    public async Task<GeneralRepsonse> DeleteUser(int id)
    {
        var httpClient = await getHttpClient.GetPrivateHttpClinet();
        var result = await httpClient.DeleteAsync($"{AuthUrl}/delete-user/{id}");
        if (!result.IsSuccessStatusCode) return new GeneralRepsonse(false, "Error occurred");

        return await result.Content.ReadFromJsonAsync<GeneralRepsonse>()!;
    }
}
