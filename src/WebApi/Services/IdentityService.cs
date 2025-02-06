using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using WebApi.Common.Contracts;
using WebApi.Data;
using WebApi.Domain;

namespace WebApi.Services;

public class IdentityService : IIdentityService
{
    private readonly IDistributedCache _cache;
    private readonly AppDbContext _context;
    private readonly ILogger<IdentityService> _logger;
    private readonly ITokenClaimsService _tokenClaimsService;
    private readonly IUserRepository _userRepository;

    public IdentityService(AppDbContext context, IUserRepository userRepository, ITokenClaimsService tokenClaimsService,
        ILogger<IdentityService> logger, IDistributedCache cache)
    {
        _context = context;
        _userRepository = userRepository;
        _tokenClaimsService = tokenClaimsService;
        _logger = logger;
        _cache = cache;
    }

    public async Task AuthenticateRequestAsync(string username)
    {
        var r = new Random();
        var code = r.Next(1000, 9999);

        var smsData = new SmsData
        {
            Code = code,
            PhoneNumber = username
        };

        var cacheKey = $"PasswordReset:{username}";
        var expiration = TimeSpan.FromMinutes(15);

        await _cache.SetStringAsync(
            cacheKey,
            JsonSerializer.Serialize(smsData),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            });

        _logger.LogInformation($"Authenticate request for phone number: {username} with 4-digit code: {code}");

        // TODO[#1]: implement sms sender service
    }

    public async Task<string> AuthenticateAsync(string username, int code)
    {
        var cacheKey = $"PasswordReset:{username}";
        var cachedData = await _cache.GetStringAsync(cacheKey);

        if (cachedData == null)
            throw new ArgumentException("Password reset code expired or not found.");

        var jsonElement = JsonSerializer.Deserialize<JsonElement>(cachedData);

        if (!jsonElement.TryGetProperty("Code", out var codeProperty) || codeProperty.GetInt32() != code)
            throw new InvalidOperationException("Invalid password reset code.");

        var user = await _userRepository.GetUserByUsername(username);

        if (user == null)
        {
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                PhoneNumber = username,
                Name = "Новый пользователь",
                PictuireUrl = "",
                CreationDate = DateTimeOffset.Now,
                Role = "User"
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
        }

        var token = await _tokenClaimsService.GetTokenAsync(username);

        return token;
    }
}

public class SmsData
{
    public int Code { get; set; }
    public string PhoneNumber { get; set; }
}