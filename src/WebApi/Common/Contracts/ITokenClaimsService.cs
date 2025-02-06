namespace WebApi.Common.Contracts;

public interface ITokenClaimsService
{
    Task<string> GetTokenAsync(string username);
}