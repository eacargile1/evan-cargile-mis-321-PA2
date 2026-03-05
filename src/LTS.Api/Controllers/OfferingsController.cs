using LTS.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LTS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OfferingsController(IOfferingRepository repo) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetOfferings(
        [FromQuery] OfferingType? type,
        [FromQuery] int? trainerId,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] string? breed,
        CancellationToken ct)
    {
        var offerings = await repo.GetOfferingsAsync(type, trainerId, from, to, breed, ct);
        return Ok(offerings.Select(Map));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var o = await repo.GetByIdAsync(id, ct);
        return o == null ? NotFound() : Ok(Map(o));
    }

    [HttpGet("trainer/{trainerId:int}")]
    public async Task<IActionResult> GetByTrainer(int trainerId, CancellationToken ct)
    {
        var offerings = await repo.GetByTrainerAsync(trainerId, ct);
        return Ok(offerings.Select(Map));
    }

    [HttpPost]
    [Authorize(Roles = "Trainer,Admin")]
    public async Task<IActionResult> Create([FromBody] CreateOfferingRequest req, CancellationToken ct)
    {
        if (req.Price < 30m) return BadRequest("Minimum session price is $30");

        var offering = new Offering
        {
            TrainerProfileId = req.TrainerProfileId,
            Type = req.Type,
            Title = req.Title,
            Description = req.Description,
            StartDateTime = req.StartDateTime,
            EndDateTime = req.EndDateTime,
            Capacity = req.Capacity,
            MinEnrollment = req.MinEnrollment > 0 ? req.MinEnrollment : 1,
            Price = req.Price,
            Location = req.Location,
            AllowedBreeds = req.AllowedBreeds
        };
        await repo.AddAsync(offering, ct);
        return CreatedAtAction(nameof(GetById), new { id = offering.Id }, Map(offering));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Trainer,Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateOfferingRequest req, CancellationToken ct)
    {
        var o = await repo.GetByIdAsync(id, ct);
        if (o == null) return NotFound();
        if (req.Price < 30m) return BadRequest("Minimum session price is $30");

        o.Title = req.Title;
        o.Description = req.Description;
        o.StartDateTime = req.StartDateTime;
        o.EndDateTime = req.EndDateTime;
        o.Capacity = req.Capacity;
        o.MinEnrollment = req.MinEnrollment > 0 ? req.MinEnrollment : 1;
        o.Price = req.Price;
        o.Location = req.Location;
        o.AllowedBreeds = req.AllowedBreeds;

        await repo.UpdateAsync(o, ct);
        return Ok(Map(o));
    }

    [HttpPatch("{id:int}/cancel")]
    [Authorize(Roles = "Trainer,Admin")]
    public async Task<IActionResult> Cancel(int id, CancellationToken ct)
    {
        var o = await repo.GetByIdAsync(id, ct);
        if (o == null) return NotFound();
        o.Status = OfferingStatus.Cancelled;
        await repo.UpdateAsync(o, ct);
        return Ok(new { o.Id, o.Status });
    }

    static object Map(Offering o)
    {
        var confirmed = o.Bookings?.Count(b => b.Status == BookingStatus.Confirmed) ?? 0;
        return new
        {
            o.Id, o.Type, o.Title, o.Description,
            o.StartDateTime, o.EndDateTime,
            o.Capacity, o.MinEnrollment, o.Price,
            o.Location, o.AllowedBreeds, o.Status,
            BookedCount = confirmed,
            SpotsLeft = o.Capacity - confirmed,
            AtRisk = confirmed < o.MinEnrollment,
            PlatformFee = Math.Round(o.Price * 0.03m, 2),
            TrainerPayout = Math.Round(o.Price * 0.97m, 2),
            Trainer = o.TrainerProfile == null ? null : new
            {
                o.TrainerProfile.Id,
                o.TrainerProfile.DisplayName,
                o.TrainerProfile.Specialties,
                o.TrainerProfile.BreedSpecialties
            },
            AverageRating = o.Reviews?.Any() == true
                ? Math.Round((double)o.Reviews.Average(r => r.Rating), 1)
                : (double?)null
        };
    }
}

public record CreateOfferingRequest(int TrainerProfileId, OfferingType Type, string Title, string? Description, DateTime StartDateTime, DateTime EndDateTime, int Capacity, int MinEnrollment, decimal Price, string? Location, string? AllowedBreeds);
public record UpdateOfferingRequest(string Title, string? Description, DateTime StartDateTime, DateTime EndDateTime, int Capacity, int MinEnrollment, decimal Price, string? Location, string? AllowedBreeds);
