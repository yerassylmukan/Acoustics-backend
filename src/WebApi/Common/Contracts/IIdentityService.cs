namespace WebApi.Common.Contracts;

public interface IIdentityService
{
    Task AuthenticateRequestAsync(string username);
    Task<string> AuthenticateAsync(string username, int code);
}