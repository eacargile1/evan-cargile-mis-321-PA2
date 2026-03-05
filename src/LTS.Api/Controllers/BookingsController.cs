using System.Security.Claims;
using LTS.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LTS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController(IBookingRepository bookingRepo, IOfferingRepository offeringRepo) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll([FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] int? trainerId, CancellationToken ct)
    {
        var bookings = await bookingRepo.GetAllAsync(from, to, trainerId, ct);
        return Ok(bookings.Select(Map));
    }

    [HttpGet("my")]
    [Authorize]
    public async Task<IActionResult> GetMine(CancellationToken ct)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email)) return Unauthorized();
        var bookings = await bookingRepo.GetByEmailAsync(email, ct);
        return Ok(bookings.Select(Map));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var b = await bookingRepo.GetByIdAsync(id, ct);
        return b == null ? NotFound() : Ok(Map(b));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookingRequest req, CancellationToken ct)
    {
        var offering = await offeringRepo.GetByIdAsync(req.OfferingId, ct);
        if (offering == null) return NotFound("Offering not found");
        if (offering.Status != OfferingStatus.Active) return BadRequest("Offering is not available");

        var confirmed = offering.Bookings?.Count(b => b.Status == BookingStatus.Confirmed) ?? 0;
        if (confirmed >= offering.Capacity) return BadRequest("This offering is fully booked");

        var booking = new Booking
        {
            OfferingId = req.OfferingId,
            CustomerName = req.CustomerName,
            CustomerEmail = req.CustomerEmail,
            CustomerPhone = req.CustomerPhone,
            PetId = req.PetId
        };
        await bookingRepo.AddAsync(booking, ct);
        var created = await bookingRepo.GetByIdAsync(booking.Id, ct);
        return CreatedAtAction(nameof(GetById), new { id = booking.Id }, created != null ? Map(created) : new { booking.Id });
    }

    [HttpPatch("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id, CancellationToken ct)
    {
        var b = await bookingRepo.GetByIdAsync(id, ct);
        if (b == null) return NotFound();
        b.Status = BookingStatus.Cancelled;
        await bookingRepo.UpdateAsync(b, ct);
        return Ok(new { b.Id, b.Status });
    }

    static object Map(Booking b) => new
    {
        b.Id, b.OfferingId, b.CustomerName, b.CustomerEmail,
        b.CustomerPhone, b.PetId, b.Status, b.CreatedAt,
        Pet = b.Pet == null ? null : new { b.Pet.Name, b.Pet.Breed },
        HasReview = b.Review != null,
        Offering = b.Offering == null ? null : new
        {
            b.Offering.Title, b.Offering.Type,
            b.Offering.StartDateTime, b.Offering.EndDateTime,
            b.Offering.Price,
            Trainer = b.Offering.TrainerProfile == null ? null : new { b.Offering.TrainerProfile.DisplayName }
        }
    };
}

public record CreateBookingRequest(int OfferingId, string CustomerName, string CustomerEmail, string CustomerPhone, int? PetId);
