namespace LTS.Core;

public class Pet
{
    public int Id { get; set; }
    public int OwnerId { get; set; }
    public User Owner { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public string Breed { get; set; } = string.Empty;
    public bool IsNeutered { get; set; }
    public bool IsSpayed { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Notes { get; set; }

    public ICollection<Booking> Bookings { get; set; } = [];
}
