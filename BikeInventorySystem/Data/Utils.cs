using System.Security.Cryptography;

namespace BikeInventorySystem.Data;


/* Utils Method*/
public static class Utils
{
    private const char _segmentDelimiter = ':';

    /* HashSecret Method*/
    public static string HashSecret(string input)
    {
        var saltSize = 16;
        var iterations = 100_000;
        var keySize = 32;
        HashAlgorithmName algorithm = HashAlgorithmName.SHA256;
        byte[] salt = RandomNumberGenerator.GetBytes(saltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(input, salt, iterations, algorithm, keySize);

        return string.Join(
            _segmentDelimiter,
            Convert.ToHexString(hash),
            Convert.ToHexString(salt),
            iterations,
            algorithm
        );
    }

    /* VerifyHash Method*/
    public static bool VerifyHash(string input, string hashString)
    {
        string[] segments = hashString.Split(_segmentDelimiter);
        byte[] hash = Convert.FromHexString(segments[0]);
        byte[] salt = Convert.FromHexString(segments[1]);
        int iterations = int.Parse(segments[2]);
        HashAlgorithmName algorithm = new(segments[3]);
        byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
            input,
            salt,
            iterations,
            algorithm,
            hash.Length
        );

        return CryptographicOperations.FixedTimeEquals(inputHash, hash);
    }

    /* GetAppDirectoryPath Method*/
    public static string GetAppDirectoryPath()
    {
        return @"D:";

    }

    /* GetAppUsersFilePath All Method*/
    public static string GetAppUsersFilePath()
    {
        return Path.Combine(GetAppDirectoryPath(), "users.json");
    }

    /* GetAppUsersFilePath All Method*/
    public static string GetTodosFilePath(Guid userId)
    {
        return Path.Combine(GetAppDirectoryPath(), userId.ToString() + "_todos.json");
    }

    /* GetAppUsersFilePath All Method*/
    public static string GetInventoryFilePath()
    {
        return Path.Combine(GetAppDirectoryPath(),  "inventory.json");
    }

    /* GetAppUsersFilePath All Method*/
    public static string GetWithdrawlFilePath(Guid userId)
    {
        return Path.Combine(GetAppDirectoryPath(), "withdrawl.json");
    }

    /* GetAppUsersFilePath All Method*/
    public static string GetApprovedFilePath(Guid userId)
    {
        return Path.Combine(GetAppDirectoryPath(), "approved.json");
    }

}
