using BaseLibrary.DTOs;
using BaseLibrary.Responses;

namespace ServerLibrary.Repositories.Contracts;

public interface IUserAccount
{
    Task<GeneralRepsonse> CreateAsync(Register user);
    Task<LoginResponse> SignInAsync(Login user);
}
