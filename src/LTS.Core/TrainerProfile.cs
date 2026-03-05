namespace LTS.Core;

public class TrainerProfile
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public string DisplayName { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string Specialties { get; set; } = string.Empty;
    public string? BreedSpecialties { get; set; } // Comma-separated breeds they prefer
    public string? PhotoUrl { get; set; }

    public ICollection<Offering> Offerings { get; set; } = [];
    public ICollection<Package> Packages { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];
}
