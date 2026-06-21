using System.Security.Cryptography;
using System.Text;

namespace Subscription_Control_Backend.Application.Services;

public static class PasswordHashService
{
    public static string Hash(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password darf nicht leer sein.", nameof(password));
        }

        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
