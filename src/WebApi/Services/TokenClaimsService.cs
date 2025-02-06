using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebApi.Common;
using WebApi.Common.Contracts;
using WebApi.Common.Exceptions;

namespace WebApi.Services;

public class TokenClaimService : ITokenClaimsService
{
    private readonly IUserRepository _userRepository;

    public TokenClaimService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<string> GetTokenAsync(string username)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(JwtOptions.JWT_SECRET_KEY);
        var user = await _userRepository.GetUserByUsername(username);
        if (user == null) throw new UserNotFoundException(username);
        var role = await _userRepository.GetRoleByUsername(user.PhoneNumber);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.PhoneNumber),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(30),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}