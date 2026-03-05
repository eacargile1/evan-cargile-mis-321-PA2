using LTS.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LTS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController(IReviewRepository repo) : ControllerBase
{
    [HttpGet("trainer/{trainerId:int}")]
    public async Task<IActionResult> GetByTrainer(int trainerId, [FromQuery] bool includeHidden = false, CancellationToken ct = default)
    {
        var reviews = await repo.GetByTrainerAsync(trainerId, includeHidden, ct);
        return Ok(reviews.Select(Map));
    }

    [HttpGet("flagged")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetFlagged(CancellationToken ct)
    {
        var reviews = await repo.GetFlaggedAsync(ct);
        return Ok(reviews.Select(r => MapAdmin(r)));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateReviewRequest req, CancellationToken ct)
    {
        if (req.Rating < 1 || req.Rating > 5)
            return BadRequest("Rating must be 1–5");

        var review = new Review
        {
            TrainerProfileId = req.TrainerProfileId,
            BookingId = req.BookingId,
            CustomerName = req.CustomerName,
            Rating = req.Rating,
            Comment = req.Comment
        };
        await repo.AddAsync(review, ct);
        return CreatedAtAction(nameof(GetByTrainer), new { trainerId = review.TrainerProfileId }, Map(review));
    }

    [HttpPost("{id:int}/flag")]
    [Authorize]
    public async Task<IActionResult> Flag(int id, [FromBody] FlagRequest req, CancellationToken ct)
    {
        var review = await repo.GetByIdAsync(id, ct);
        if (review == null) return NotFound();
        review.IsFlagged = true;
        review.IsHidden = true;
        review.FlagReason = req.Reason;
        await repo.UpdateAsync(review, ct);
        return Ok(MapAdmin(review));
    }

    [HttpPost("{id:int}/unflag")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Unflag(int id, CancellationToken ct)
    {
        var review = await repo.GetByIdAsync(id, ct);
        if (review == null) return NotFound();
        review.IsFlagged = false;
        review.IsHidden = false;
        review.FlagReason = null;
        await repo.UpdateAsync(review, ct);
        return Ok(MapAdmin(review));
    }

    static object Map(Review r) => new
    {
        r.Id, r.TrainerProfileId, r.CustomerName,
        r.Rating, r.Comment, r.CreatedAt
    };

    static object MapAdmin(Review r) => new
    {
        r.Id, r.TrainerProfileId, r.BookingId,
        r.CustomerName, r.Rating, r.Comment,
        r.IsFlagged, r.IsHidden, r.FlagReason, r.CreatedAt,
        TrainerName = r.TrainerProfile?.DisplayName
    };
}

public record CreateReviewRequest(int TrainerProfileId, int BookingId, string CustomerName, int Rating, string Comment);
public record FlagRequest(string? Reason);
