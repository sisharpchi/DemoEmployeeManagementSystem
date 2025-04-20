using BaseLibrary.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ClientLibrary.Helpers;

public class CustomAuthenticationStateProvider(LocalStorageService localStorageService) : AuthenticationStateProvider
{
    private readonly ClaimsPrincipal anonymous = new(new ClaimsIdentity());

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var stringToken = await localStorageService.GetToken();
        if (string.IsNullOrEmpty(stringToken)) return await Task.FromResult(new AuthenticationState(anonymous));

        var deserializeToken = Serializations.DeserializeJsonString<UserSession>(stringToken);
        if (deserializeToken is null) return await Task.FromResult(new AuthenticationState(anonymous));

        var getUserClaims = DecryptToken(deserializeToken.Token!);
        if (getUserClaims is null) return await Task.FromResult(new AuthenticationState(anonymous));

        var claimsPrincipal = SetClaimPricipal(getUserClaims);
        return await Task.FromResult(new AuthenticationState(claimsPrincipal));
    }

    public async Task UpdateAuthenticationState(UserSession userSession)
    {
        var claimsPrincipal = new ClaimsPrincipal();
        if (userSession is not null || userSession.RefreshToken is not null)
        {
            var serializeSession = Serializations.SerializeObj(userSession);
            await localStorageService.SetToken(serializeSession);
            var getUserClaims = DecryptToken(userSession.Token!);
            claimsPrincipal = SetClaimPricipal(getUserClaims);
        }
        else
        {
            await localStorageService.RemoveToken();
        }
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    private static ClaimsPrincipal SetClaimPricipal(CustomUserClaims claims)
    {
        if (claims.Email is null) return new ClaimsPrincipal();
        return new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, claims.Id!),
            new Claim(ClaimTypes.Name, claims.Name!),
            new Claim(ClaimTypes.Email, claims.Email!),
            new Claim(ClaimTypes.Role, claims.Role!)
        }, "JwtAuth"));
    }

    private static CustomUserClaims DecryptToken(string jwtToken)
    {
        if (string.IsNullOrEmpty(jwtToken)) return new CustomUserClaims();

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwtToken);

        var userId = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.NameIdentifier);
        var name = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Name);
        var email = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Email);
        var role = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Role);
        return new CustomUserClaims(userId!.Value!, name!.Value, email!.Value, role!.Value); 
    }
}
