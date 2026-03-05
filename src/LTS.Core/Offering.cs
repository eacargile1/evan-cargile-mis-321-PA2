namespace LTS.Core;

public class Offering
{
    public int Id { get; set; }
    public int TrainerProfileId { get; set; }
    public TrainerProfile TrainerProfile { get; set; } = null!;

    public OfferingType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public int Capacity { get; set; } = 1;
    public int MinEnrollment { get; set; } = 1;
    public decimal Price { get; set; } = 30m;
    public string? Location { get; set; }
    public string? AllowedBreeds { get; set; } // Comma-separated; empty = all breeds
    public OfferingStatus Status { get; set; } = OfferingStatus.Active;

    public ICollection<Booking> Bookings { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];
}

public enum OfferingType
{
    Class,
    Individual
}

public enum OfferingStatus
{
    Active,
    Cancelled,
    Completed
}
