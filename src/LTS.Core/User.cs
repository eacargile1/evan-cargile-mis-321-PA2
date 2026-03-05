namespace LTS.Core;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public TrainerProfile? TrainerProfile { get; set; }
    public ICollection<Pet> Pets { get; set; } = [];
}

public enum UserRole
{
    Admin,
    Trainer,
    PetOwner
}
