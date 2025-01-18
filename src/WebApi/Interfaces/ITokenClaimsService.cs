namespace WebApi.Interfaces;

public interface ITokenClaimsService
{
    Task<string> GetTokenAsync(string userName);
    string GetAnonymousToken();
}