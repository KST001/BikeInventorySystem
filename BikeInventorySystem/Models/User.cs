namespace BikeInventorySystem.Models;


/* User Method*/
public class User
{
    /*  User All Access Modifier */
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public Role Role { get; set; }
    public bool HasInitialPassword { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public Guid CreatedBy { get; set; }
}
