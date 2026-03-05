using System.Security.Claims;
using LTS.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LTS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrainersController(ITrainerProfileRepository repo, IReviewRepository reviewRepo) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? breed, CancellationToken ct)
    {
        var trainers = await repo.GetAllAsync(ct);
        if (!string.IsNullOrWhiteSpace(breed))
            trainers = trainers.Where(t => string.IsNullOrEmpty(t.BreedSpecialties) || t.BreedSpecialties.Contains(breed, StringComparison.OrdinalIgnoreCase)).ToList();
        return Ok(trainers.Select(Map));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var t = await repo.GetByIdAsync(id, ct);
        if (t == null) return NotFound();
        var reviews = await reviewRepo.GetByTrainerAsync(id, ct: ct);
        return Ok(MapDetail(t, reviews));
    }

    [HttpGet("me")]
    [Authorize(Roles = "Trainer")]
    public async Task<IActionResult> GetMe(CancellationToken ct)
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();
        var profile = await repo.GetByUserIdAsync(userId, ct);
        if (profile == null) return NotFound("No trainer profile found for this account");
        var reviews = await reviewRepo.GetByTrainerAsync(profile.Id, ct: ct);
        return Ok(MapDetail(profile, reviews));
    }

    [HttpPost]
    [Authorize(Roles = "Trainer,Admin")]
    public async Task<IActionResult> Create([FromBody] CreateTrainerRequest req, CancellationToken ct)
    {
        var profile = new TrainerProfile
        {
            UserId = req.UserId,
            DisplayName = req.DisplayName,
            Bio = req.Bio,
            Specialties = req.Specialties ?? string.Empty,
            BreedSpecialties = req.BreedSpecialties,
            PhotoUrl = req.PhotoUrl
        };
        await repo.AddAsync(profile, ct);
        return CreatedAtAction(nameof(GetById), new { id = profile.Id }, Map(profile));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Trainer,Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTrainerRequest req, CancellationToken ct)
    {
        var t = await repo.GetByIdAsync(id, ct);
        if (t == null) return NotFound();
        t.DisplayName = req.DisplayName;
        t.Bio = req.Bio;
        t.Specialties = req.Specialties ?? string.Empty;
        t.BreedSpecialties = req.BreedSpecialties;
        t.PhotoUrl = req.PhotoUrl;
        await repo.UpdateAsync(t, ct);
        return Ok(Map(t));
    }

    static object Map(TrainerProfile t) => new
    {
        t.Id, t.DisplayName, t.Bio, t.Specialties, t.BreedSpecialties, t.PhotoUrl
    };

    static object MapDetail(TrainerProfile t, IReadOnlyList<Review> reviews) => new
    {
        t.Id, t.DisplayName, t.Bio, t.Specialties, t.BreedSpecialties, t.PhotoUrl,
        AverageRating = reviews.Any() ? Math.Round(reviews.Average(r => r.Rating), 1) : (double?)null,
        ReviewCount = reviews.Count,
        Reviews = reviews.Select(r => new { r.Id, r.CustomerName, r.Rating, r.Comment, r.CreatedAt })
    };
}

public record CreateTrainerRequest(int UserId, string DisplayName, string Bio, string? Specialties, string? BreedSpecialties, string? PhotoUrl);
public record UpdateTrainerRequest(string DisplayName, string Bio, string? Specialties, string? BreedSpecialties, string? PhotoUrl);
