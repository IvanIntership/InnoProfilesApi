using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using ProfilesApi.Application.Interfaces;

namespace ProfilesApi.Application.Services;

public class PasswordHasher : IPasswordHasher
{
    private readonly byte[] _keyBytes;
    
    public PasswordHasher(IConfiguration configuration)
    {
        var pepper = configuration["PasswordSettings:Key"] 
                     ?? throw new InvalidOperationException("Password Key secret is missing in configuration!");

        _keyBytes = Encoding.UTF8.GetBytes(pepper);
    }
    public string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty.", nameof(password));
        
        byte[] keyPassword = HMACSHA256.HashData(_keyBytes, Encoding.UTF8.GetBytes(password));

        byte[] salt = RandomNumberGenerator.GetBytes(16);

        byte[] hash = KeyDerivation.Pbkdf2(
            password: Convert.ToBase64String(keyPassword),
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 600000,
            numBytesRequested: 32);

        byte[] payload = new byte[16 + 32];
        Buffer.BlockCopy(salt, 0, payload, 0, 16);
        Buffer.BlockCopy(hash, 0, payload, 16, 32);
        
        return Convert.ToBase64String(payload);
    }
}