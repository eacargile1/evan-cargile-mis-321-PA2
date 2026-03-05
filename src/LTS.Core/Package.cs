namespace LTS.Core;

public class Package
{
    public int Id { get; set; }
    public int TrainerProfileId { get; set; }
    public TrainerProfile TrainerProfile { get; set; } = null!;

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int NumSessions { get; set; }
    public decimal TotalPrice { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<PackageBooking> PackageBookings { get; set; } = [];
}

public class PackageBooking
{
    public int Id { get; set; }
    public int PackageId { get; set; }
    public Package Package { get; set; } = null!;

    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public int? PetId { get; set; }
    public Pet? Pet { get; set; }

    public int SessionsUsed { get; set; } = 0;
    public BookingStatus Status { get; set; } = BookingStatus.Confirmed;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
