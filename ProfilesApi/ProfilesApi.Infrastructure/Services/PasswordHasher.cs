using System.Security.Cryptography;
using ProfilesApi.Application.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ProfilesApi.Infrastructure.Services;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty.", nameof(password));

        byte[] salt = RandomNumberGenerator.GetBytes(16);

        byte[] hash = KeyDerivation.Pbkdf2(
            password: password,
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