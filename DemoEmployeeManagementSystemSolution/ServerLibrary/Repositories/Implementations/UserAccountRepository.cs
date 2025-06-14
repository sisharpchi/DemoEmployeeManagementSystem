using BaseLibrary.DTOs;
using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServerLibrary.Data;
using ServerLibrary.Helpers;
using ServerLibrary.Repositories.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ServerLibrary.Repositories.Implementations;

public class UserAccountRepository(IOptions<JwtSection> config, AppDbContext appDbContext) : IUserAccount
{
    public async Task<GeneralRepsonse> CreateAsync(Register user)
    {
        if (user is null) return new GeneralRepsonse(false, "Model is empty");

        var checkUser = await FindUserByEmail(user.Email);
        if (checkUser is not null) return new GeneralRepsonse(false, "User registered already");

        var applicationUser = await AddToDatabase(new ApplicationUser()
        {
            FullName = user.FullName,
            Email = user.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(user.Password)
        });

        var checkAdminRole = await appDbContext.SystemRoles.FirstOrDefaultAsync(_ => _.Name!.Equals(Constants.Admin));
        if (checkAdminRole is null)
        {
            var createAdminRole = await AddToDatabase(new SystemRole() { Name = Constants.Admin });
            await AddToDatabase(new UserRole() { RoleId = createAdminRole.Id, UserId = applicationUser.Id });
            return new GeneralRepsonse(true, "Account created");
        }

        var checkUserRole = await appDbContext.SystemRoles.FirstOrDefaultAsync(_ => _.Name!.Equals(Constants.User));
        SystemRole response = new();
        if (checkUserRole is null)
        {
            response = await AddToDatabase(new SystemRole() { Name = Constants.User });
            await AddToDatabase(new UserRole() { RoleId = response.Id, UserId = applicationUser.Id });
        }
        else
        {
            await AddToDatabase(new UserRole() { RoleId = checkUserRole.Id, UserId = applicationUser.Id });
        }
        return new GeneralRepsonse(true, "Account created");
    }

    public async Task<LoginResponse> SignInAsync(Login user)
    {
        if (user is null) return new LoginResponse(false, "Model is empty");

        var applicationUser = await FindUserByEmail(user.Email);
        if (applicationUser is null) return new LoginResponse(false, "User not found");

        if (!BCrypt.Net.BCrypt.Verify(user.Password, applicationUser.Password))
            return new LoginResponse(false, "Email/Password not valid");

        var getUserRole = await FindUserRole(applicationUser.Id);
        if (getUserRole is null) return new LoginResponse(false, "User role not found");

        var getRoleName = await FindRoleName(getUserRole.RoleId);
        if (getRoleName is null) return new LoginResponse(false, "User role not found");

        var jwtToken = GenerateToken(applicationUser, getRoleName!.Name!);
        var refreshToken = GenerateRefreshToken();

        var findUser = await appDbContext.RefreshTokenInfos.FirstOrDefaultAsync(_ => _.UserId == applicationUser.Id);
        if (findUser is not null)
        {
            findUser!.Token = refreshToken;
            await appDbContext.SaveChangesAsync();
        }
        else
        {
            await AddToDatabase(new RefreshTokenInfo()
            {
                Token = refreshToken,
                UserId = applicationUser.Id
            });
        }   
        return new LoginResponse(true, "Login successful", jwtToken, refreshToken);
    }

    private string GenerateToken(ApplicationUser user, string role)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Value.Key!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var userClaims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName!),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Role, role!)
        };
        var token = new JwtSecurityToken(
            issuer: config.Value.Issuer,
            audience: config.Value.Audience,
            claims: userClaims,
            expires: DateTime.Now.AddSeconds(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<UserRole> FindUserRole(int userId) => await appDbContext.UserRoles.FirstOrDefaultAsync(_ => _.UserId == userId);
    private async Task<SystemRole> FindRoleName(int roleId) => await appDbContext.SystemRoles.FirstOrDefaultAsync(_ => _.Id == roleId);
    private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    private async Task<ApplicationUser> FindUserByEmail(string email) =>
            await appDbContext.ApplicationUsers.FirstOrDefaultAsync(_ => _.Email!.ToLower()!.Equals(email!.ToLower()));
    
    private async Task<T> AddToDatabase<T>(T model)
    {
        var result = await appDbContext.AddAsync(model!);
        await appDbContext.SaveChangesAsync();
        return (T)result.Entity;
        throw new Exception();
    }
    public async Task<LoginResponse> RefreshTokenAsync(RefreshToken token)
    {
        if (token is null) return new LoginResponse(false, "Model is empty");

        var findToken = await appDbContext.RefreshTokenInfos.FirstOrDefaultAsync(_ => _.Token!.Equals(token.Token));
        if (findToken is null) return new LoginResponse(false, "Refresh token is required");

        var user = await appDbContext.ApplicationUsers.FirstOrDefaultAsync(_ => _.Id == findToken.UserId);
        if (user is null) return new LoginResponse(false, "Refresh token could not be generated because user not found");

        var userRole = await FindUserRole(user.Id);
        var roleName = await FindRoleName(userRole.RoleId);
        var jwtToken = GenerateToken(user, roleName.Name!);
        var refreshToken = GenerateRefreshToken();

        var updateRefreshToken = await appDbContext.RefreshTokenInfos.FirstOrDefaultAsync(_ => _.UserId == user.Id);
        if (updateRefreshToken is null) return new LoginResponse(false, "Refresh token could not be generated because user has not sign in");

        updateRefreshToken.Token = refreshToken;
        await appDbContext.SaveChangesAsync();
        return new LoginResponse(true, "Token refreshed successfully", jwtToken, refreshToken);
    }

    public async Task<List<ManagerUser>> GetUsers()
    {
        var allUsers = await GetApplicationUsers();
        var allUserRoles = await UserRoles();
        var allRoles = await SystemRoles();

        if (allUsers.Count == 0 || allRoles.Count == 0) return null!;

        var users = new List<ManagerUser>();

        foreach (var user in allUsers)
        {
            var userRole = allUserRoles.FirstOrDefault(u => u.UserId == user.Id);
            var roleName = allRoles.FirstOrDefault(r => r.Id == userRole!.RoleId);
            users.Add(new ManagerUser() { UserId = user.Id, Name = user.FullName!, Email = user.Email!, Role = roleName!.Name! });
        }
        return users;
    }

    public async Task<GeneralRepsonse> UpdateUser(ManagerUser user)
    {
        var getRole = (await SystemRoles()).FirstOrDefault(r => r.Name!.Equals(user.Role));
        var userRole = await appDbContext.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == user.UserId);
        userRole!.RoleId = getRole!.Id;
        await appDbContext.SaveChangesAsync();
        return new GeneralRepsonse(true, "User role updated successfully");
    }

    public async Task<List<SystemRole>> GetRoles() => await SystemRoles();
    
    public async Task<GeneralRepsonse> DeleteUser(int id)
    {
        var user = await appDbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == id);
        appDbContext.ApplicationUsers.Remove(user!);
        await appDbContext.SaveChangesAsync();
        return new GeneralRepsonse(true, "User successfully deleted");
    }

    private async Task<List<SystemRole>> SystemRoles() => await appDbContext.SystemRoles.AsNoTracking().ToListAsync();
    private async Task<List<UserRole>> UserRoles() => await appDbContext.UserRoles.AsNoTracking().ToListAsync();
    private async Task<List<ApplicationUser>> GetApplicationUsers() => await appDbContext.ApplicationUsers.AsNoTracking().ToListAsync();
}