namespace UExpo.Application.Utils;

public static class HashHelper
{
    public static string Hash(string value)
    {
        return BCrypt.Net.BCrypt.HashPassword(value);
    }

    public static bool Verify(string value, string hashedValue)
    {
        return BCrypt.Net.BCrypt.Verify(value, hashedValue);
    }
}
