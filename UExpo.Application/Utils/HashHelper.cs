namespace UExpo.Application.Utils;

public static class HashHelper
{
    public static string Hash(string value)
    {
        var hash = BCrypt.Net.BCrypt.HashPassword(value);

        return hash.Replace('/', '-');
    }

    public static bool Verify(string value, string hashedValue)
    {
        var parsedHashedValue = hashedValue.Replace('-', '/');

        return BCrypt.Net.BCrypt.Verify(value, parsedHashedValue);
    }
}
