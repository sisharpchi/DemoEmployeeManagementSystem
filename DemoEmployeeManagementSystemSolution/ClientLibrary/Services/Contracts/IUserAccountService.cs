using BaseLibrary.DTOs;
using BaseLibrary.Entities;
using BaseLibrary.Responses;

namespace ClientLibrary.Services.Contracts;

public interface IUserAccountService
{
    Task<GeneralRepsonse> CreateAsync(Register user);
    Task<LoginResponse> SignInAsync(Login user);
    Task<LoginResponse> RefreshTokenAsync(RefreshToken token);
    Task<List<ManagerUser>> GetUsers();
    Task<GeneralRepsonse> UpdateUser(ManagerUser user);
    Task<List<SystemRole>> GetRoles();
    Task<GeneralRepsonse> DeleteUser(int id);
}
