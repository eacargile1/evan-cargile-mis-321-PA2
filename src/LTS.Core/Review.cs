namespace LTS.Core;

public class Review
{
    public int Id { get; set; }
    public int TrainerProfileId { get; set; }
    public TrainerProfile TrainerProfile { get; set; } = null!;

    public int BookingId { get; set; }
    public Booking Booking { get; set; } = null!;

    public string CustomerName { get; set; } = string.Empty;
    public int Rating { get; set; } // 1-5
    public string Comment { get; set; } = string.Empty;

    public bool IsFlagged { get; set; }
    public bool IsHidden { get; set; }
    public string? FlagReason { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
