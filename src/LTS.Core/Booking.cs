namespace LTS.Core;

public class Booking
{
    public int Id { get; set; }
    public int OfferingId { get; set; }
    public Offering Offering { get; set; } = null!;

    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public int? PetId { get; set; }
    public Pet? Pet { get; set; }

    public BookingStatus Status { get; set; } = BookingStatus.Confirmed;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Review? Review { get; set; }
}

public enum BookingStatus
{
    Confirmed,
    Cancelled,
    Completed
}
