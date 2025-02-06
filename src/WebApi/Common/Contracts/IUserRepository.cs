using WebApi.Domain;

namespace WebApi.Common.Contracts;

public interface IUserRepository
{
    Task<User?> GetUserByUsername(string username);
    Task<string> GetRoleByUsername(string username);
}