using LTS.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LTS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PackagesController(IPackageRepository repo) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok((await repo.GetAllActiveAsync(ct)).Select(Map));

    [HttpGet("trainer/{trainerId:int}")]
    public async Task<IActionResult> GetByTrainer(int trainerId, CancellationToken ct) =>
        Ok((await repo.GetByTrainerAsync(trainerId, ct)).Select(Map));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var pkg = await repo.GetByIdAsync(id, ct);
        return pkg == null ? NotFound() : Ok(Map(pkg));
    }

    [HttpPost]
    [Authorize(Roles = "Trainer,Admin")]
    public async Task<IActionResult> Create([FromBody] CreatePackageRequest req, CancellationToken ct)
    {
        var pkg = new Package
        {
            TrainerProfileId = req.TrainerProfileId,
            Title = req.Title,
            Description = req.Description,
            NumSessions = req.NumSessions,
            TotalPrice = req.TotalPrice,
            IsActive = true
        };
        await repo.AddAsync(pkg, ct);
        return CreatedAtAction(nameof(GetById), new { id = pkg.Id }, Map(pkg));
    }

    [HttpPost("{id:int}/book")]
    public async Task<IActionResult> Book(int id, [FromBody] BookPackageRequest req, CancellationToken ct)
    {
        var pkg = await repo.GetByIdAsync(id, ct);
        if (pkg == null || !pkg.IsActive) return NotFound("Package not found or inactive");

        var booking = new PackageBooking
        {
            PackageId = id,
            CustomerName = req.CustomerName,
            CustomerEmail = req.CustomerEmail,
            CustomerPhone = req.CustomerPhone,
            PetId = req.PetId
        };
        await repo.AddBookingAsync(booking, ct);
        return Ok(new { booking.Id, booking.PackageId, booking.CustomerName, booking.CustomerEmail, booking.Status });
    }

    static object Map(Package p) => new
    {
        p.Id, p.TrainerProfileId, p.Title, p.Description,
        p.NumSessions, p.TotalPrice, p.IsActive,
        PricePerSession = p.NumSessions > 0 ? Math.Round(p.TotalPrice / p.NumSessions, 2) : 0,
        Trainer = p.TrainerProfile == null ? null : new { p.TrainerProfile.DisplayName }
    };
}

public record CreatePackageRequest(int TrainerProfileId, string Title, string? Description, int NumSessions, decimal TotalPrice);
public record BookPackageRequest(string CustomerName, string CustomerEmail, string CustomerPhone, int? PetId);
