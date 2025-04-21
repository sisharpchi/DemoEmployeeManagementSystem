using BaseLibrary.DTOs;
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

    public async Task<WeatherForecast[]> GetWeatherForecast()
    {
        var httpClient = await getHttpClient.GetPrivateHttpClinet();
        var result = await httpClient.GetFromJsonAsync<WeatherForecast[]>("api/weatherforecast");
        return result!;
    }
}
