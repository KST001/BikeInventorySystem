using System.Text.Json;

namespace BikeInventorySystem.Data;


/* UsersService Method*/
public static class UsersService
{
    public const string SeedUsername = "admin";
    public const string SeedPassword = "admin";


    /* SaveAll Method*/
    private static void SaveAll(List<User> users)
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string appUsersFilePath = Utils.GetAppUsersFilePath();

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(users);
        File.WriteAllText(appUsersFilePath, json);
    }


    /* GetAll Method*/
    public static List<User> GetAll()
    {
        string appUsersFilePath = Utils.GetAppUsersFilePath();
        if (!File.Exists(appUsersFilePath))
        {
            return new List<User>();
        }

        var json = File.ReadAllText(appUsersFilePath);

        return JsonSerializer.Deserialize<List<User>>(json);
    }


    /* Create Method*/
    public static List<User> Create(Guid userId, string username, string password, Role role)
    {
        List<User> users = GetAll();

        bool usernameExists = users.Any(x => x.Username == username);
        if (string.IsNullOrEmpty(username))
        {
            throw new Exception("Username cannot be empty");
        }
        else if (usernameExists)
        {
            throw new Exception("Username already exists.");
        }
        else if (role == Role.Admin && !MaxAdmin())
        {
            throw new Exception("Maximum Number of Admins Reached");
        }
        else
        {
            users.Add(
             new User
             {
                 Username = username,
                 PasswordHash = Utils.HashSecret(password),
                 Role = role,
                 CreatedBy = userId
             });
        }
        Console.WriteLine(users);
        SaveAll(users);
        return users;
    }



    /* MaxAdmin Method*/
    public static bool MaxAdmin()
    {
        int adminCount = GetAll().Where(x => x.Role == Role.Admin).Count();
        return adminCount < 2;
    }


    /* SeedUsers Method*/
    public static void SeedUsers()
    {
        var users = GetAll().FirstOrDefault(x => x.Role == Role.Admin);

        if (users == null)
        {
            Create(Guid.Empty, SeedUsername, SeedPassword, Role.Admin);
        }
    }


    /* GetById Method*/
    public static User GetById(Guid id)
    {
        List<User> users = GetAll();
        return users.FirstOrDefault(x => x.Id == id);
    }


    /* Delete Method*/
    public static List<User> Delete(Guid id)
    {
        List<User> users = GetAll();
        User user = users.FirstOrDefault(x => x.Id == id);

        if (user == null)
        {
            throw new Exception("User not found.");
        }
        users.Remove(user);
        SaveAll(users);

        return users;
    }


    /* Login Method*/
    public static User Login(string username, string password)
    {
        var loginErrorMessage = "Invalid username or password.";
        List<User> users = GetAll();
        User user = users.FirstOrDefault(x => x.Username == username);

        if (user == null)
        {
            throw new Exception(loginErrorMessage);
        }

        bool passwordIsValid = Utils.VerifyHash(password, user.PasswordHash);

        if (!passwordIsValid)
        {
            throw new Exception(loginErrorMessage);
        }


        Console.WriteLine("Successfully Logged in");

        return user;
    }


    /* ChangePassword Method*/
    public static User ChangePassword(Guid id, string currentPassword, string newPassword)
    {
        if (currentPassword == newPassword)
        {
            throw new Exception("New password must be different from current password.");
        }

        List<User> users = GetAll();
        User user = users.FirstOrDefault(x => x.Id == id);

        if (user == null)
        {
            throw new Exception("User not found.");
        }

        bool passwordIsValid = Utils.VerifyHash(currentPassword, user.PasswordHash);

        if (!passwordIsValid)
        {
            throw new Exception("Incorrect current password.");
        }

        user.PasswordHash = Utils.HashSecret(newPassword);
        user.HasInitialPassword = false;
        SaveAll(users);

        return user;
    }
}
