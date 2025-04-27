
using BaseLibrary.DTOs;
using ClientLibrary.Services.Contracts;
using System.Net;

namespace ClientLibrary.Helpers;

public class CustomHttpHandler(GetHttpClient getHttpClient, LocalStorageService localStorageService, IUserAccountService accountService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var loginUrl = request.RequestUri!.AbsoluteUri.Contains("login");
        var registerUrl = request.RequestUri!.AbsoluteUri!.Contains("register");
        var refreshTokenUrl = request.RequestUri!.AbsoluteUri!.Contains("refresh-token");

        if (loginUrl || registerUrl || refreshTokenUrl) return await base.SendAsync(request, cancellationToken);

        var result = await  base.SendAsync(request, cancellationToken);
        if(result.StatusCode == HttpStatusCode.Unauthorized)
        {
            //get token from localStorage
            var stringToken = await localStorageService.GetToken();
            if (stringToken is null) return result;

            //check if the header containers token
            string token = string.Empty;
            try { token = request.Headers.Authorization!.Parameter!; }
            catch { }

            var deserializedToken = Serializations.DeserializeJsonString<UserSession>(stringToken);
            if (deserializedToken is null) return result;
             
            if (string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", deserializedToken.Token);
                return await base.SendAsync(request, cancellationToken);
            }

            var newJwtToken = await GetReshToken(deserializedToken.RefreshToken!);
            if (string.IsNullOrEmpty(newJwtToken)) return result;

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", newJwtToken);
            return await base.SendAsync(request, cancellationToken);
        }
        return result;
    }

    private async Task<string> GetReshToken(string refreshToken)
    {
        var result = await accountService.RefreshTokenAsync(new RefreshToken() { Token = refreshToken });
        string serializedToken = Serializations.SerializeObj(new UserSession() { Token = result.Token, RefreshToken = result.RefreshToken});
        await localStorageService.SetToken(serializedToken);
        return result.Token;
    }
}
