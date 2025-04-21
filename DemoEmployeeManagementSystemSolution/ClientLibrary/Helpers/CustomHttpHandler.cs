
using BaseLibrary.DTOs;
using System.Net;

namespace ClientLibrary.Helpers;

public class CustomHttpHandler(GetHttpClient getHttpClient, LocalStorageService localStorageService) : DelegatingHandler
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
        }
        return result;
    }
}
