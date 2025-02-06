using Microsoft.EntityFrameworkCore;
using WebApi.Common.Contracts;
using WebApi.Common.Exceptions;
using WebApi.Domain;

namespace WebApi.Data;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == username);
    }

    public async Task<string> GetRoleByUsername(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == username);

        if (user == null) throw new UserNotFoundException(username);

        return user.Role;
    }
}