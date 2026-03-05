using LTS.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LTS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class ReportsController(IBookingRepository bookingRepo, IOfferingRepository offeringRepo, IPetRepository petRepo, IPackageRepository packageRepo) : ControllerBase
{
    private const decimal PlatformFeeRate = 0.03m;

    [HttpGet("bookings")]
    public async Task<IActionResult> BookingsReport([FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] int? trainerId, [FromQuery] string? type, CancellationToken ct)
    {
        var bookings = await bookingRepo.GetAllAsync(from, to, trainerId, ct);
        if (!string.IsNullOrEmpty(type) && Enum.TryParse<OfferingType>(type, true, out var offeringType))
            bookings = bookings.Where(b => b.Offering.Type == offeringType).ToList();

        var byDate = bookings
            .GroupBy(b => b.Offering.StartDateTime.Date)
            .Select(g => new { Date = g.Key, Count = g.Count(), Classes = g.Count(b => b.Offering.Type == OfferingType.Class), Individual = g.Count(b => b.Offering.Type == OfferingType.Individual) })
            .OrderBy(x => x.Date).ToList();

        var byTrainer = bookings
            .GroupBy(b => b.Offering.TrainerProfileId)
            .Select(g => new { TrainerName = g.First().Offering.TrainerProfile?.DisplayName, Count = g.Count(), Classes = g.Count(b => b.Offering.Type == OfferingType.Class), Individual = g.Count(b => b.Offering.Type == OfferingType.Individual) })
            .ToList();

        return Ok(new
        {
            Summary = new { Total = bookings.Count, Classes = bookings.Count(b => b.Offering.Type == OfferingType.Class), Individual = bookings.Count(b => b.Offering.Type == OfferingType.Individual) },
            ByDate = byDate,
            ByTrainer = byTrainer
        });
    }

    [HttpGet("revenue")]
    public async Task<IActionResult> RevenueReport([FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] int? trainerId, CancellationToken ct)
    {
        var bookings = await bookingRepo.GetAllAsync(from, to, trainerId, ct);
        var pkgBookings = await packageRepo.GetAllBookingsAsync(from, to, trainerId, ct);

        // Merge session bookings + package purchases per trainer
        var sessionByTrainer = bookings
            .GroupBy(b => b.Offering.TrainerProfileId)
            .ToDictionary(g => g.Key, g => (Name: g.First().Offering.TrainerProfile?.DisplayName, SessionGross: g.Sum(b => b.Offering.Price), SessionCount: g.Count()));

        var pkgByTrainer = pkgBookings
            .GroupBy(p => p.Package.TrainerProfileId)
            .ToDictionary(g => g.Key, g => (Name: g.First().Package.TrainerProfile?.DisplayName, PkgGross: g.Sum(p => p.Package.TotalPrice), PkgCount: g.Count()));

        var allTrainerIds = sessionByTrainer.Keys.Union(pkgByTrainer.Keys).ToList();

        var byTrainer = allTrainerIds.Select(tid =>
        {
            sessionByTrainer.TryGetValue(tid, out var s);
            pkgByTrainer.TryGetValue(tid, out var p);
            var name = s.Name ?? p.Name;
            var gross = s.SessionGross + p.PkgGross;
            var fee = Math.Round(gross * PlatformFeeRate, 2);
            return new
            {
                TrainerName = name,
                TrainerId = tid,
                SessionBookings = s.SessionCount,
                PackagePurchases = p.PkgCount,
                SessionRevenue = s.SessionGross,
                PackageRevenue = p.PkgGross,
                GrossRevenue = gross,
                PlatformFee = fee,
                TrainerPayout = Math.Round(gross - fee, 2)
            };
        }).ToList();

        var totalGross = byTrainer.Sum(r => r.GrossRevenue);
        var totalFee = Math.Round(totalGross * PlatformFeeRate, 2);

        return Ok(new
        {
            Summary = new
            {
                TotalSessionBookings = bookings.Count,
                TotalPackagePurchases = pkgBookings.Count,
                GrossRevenue = totalGross,
                PlatformFee = totalFee,
                TrainerPayouts = Math.Round(totalGross - totalFee, 2)
            },
            ByTrainer = byTrainer
        });
    }

    [HttpGet("business")]
    public async Task<IActionResult> BusinessReport([FromQuery] DateTime? from, [FromQuery] DateTime? to, CancellationToken ct)
    {
        var bookings = await bookingRepo.GetAllAsync(from, to, null, ct);
        var pkgBookings = await packageRepo.GetAllBookingsAsync(from, to, null, ct);

        // Who do I owe — combine session + package revenue per trainer
        var sessionByTrainer = bookings.GroupBy(b => b.Offering.TrainerProfileId)
            .ToDictionary(g => g.Key, g => (Name: g.First().Offering.TrainerProfile?.DisplayName, Gross: g.Sum(b => b.Offering.Price), Count: g.Count()));
        var pkgByTrainer = pkgBookings.GroupBy(p => p.Package.TrainerProfileId)
            .ToDictionary(g => g.Key, g => (Name: g.First().Package.TrainerProfile?.DisplayName, Gross: g.Sum(p => p.Package.TotalPrice), Count: g.Count()));

        var trainerPayouts = sessionByTrainer.Keys.Union(pkgByTrainer.Keys).Select(tid =>
        {
            sessionByTrainer.TryGetValue(tid, out var s);
            pkgByTrainer.TryGetValue(tid, out var p);
            var gross = s.Gross + p.Gross;
            return new
            {
                TrainerName = s.Name ?? p.Name,
                TrainerId = tid,
                SessionBookings = s.Count,
                PackagePurchases = p.Count,
                GrossRevenue = gross,
                AmountOwed = Math.Round(gross * (1 - PlatformFeeRate), 2)
            };
        }).ToList();

        // Projected revenue (upcoming session bookings)
        var futureBookings = bookings.Where(b => b.Offering.StartDateTime > DateTime.UtcNow).ToList();
        var projectedGross = futureBookings.Sum(b => b.Offering.Price);

        // Repeat customers — merge session + package bookings by email
        var allCustomerActivity = bookings
            .Select(b => (Email: b.CustomerEmail, Name: b.CustomerName, Spent: b.Offering.Price))
            .Concat(pkgBookings.Select(p => (Email: p.CustomerEmail, Name: p.CustomerName, Spent: p.Package.TotalPrice)))
            .GroupBy(x => x.Email)
            .Where(g => g.Count() > 1)
            .Select(g => new { Email = g.Key, Name = g.First().Name, ActivityCount = g.Count(), TotalSpent = g.Sum(x => x.Spent) })
            .OrderByDescending(x => x.ActivityCount)
            .ToList();

        return Ok(new
        {
            TrainerPayouts = trainerPayouts,
            TotalOwed = trainerPayouts.Sum(t => t.AmountOwed),
            ProjectedRevenue = new { GrossRevenue = projectedGross, PlatformFee = Math.Round(projectedGross * PlatformFeeRate, 2), UpcomingBookings = futureBookings.Count },
            RepeatCustomers = allCustomerActivity
        });
    }

    [HttpGet("offerings")]
    public async Task<IActionResult> TrainerOfferingsReport([FromQuery] int? trainerId, [FromQuery] DateTime? from, [FromQuery] DateTime? to, CancellationToken ct)
    {
        var offerings = await offeringRepo.GetOfferingsAsync(null, trainerId, from, to, null, ct);
        var allOfferings = trainerId.HasValue ? await offeringRepo.GetByTrainerAsync(trainerId.Value, ct) : offerings;

        var rows = allOfferings.Select(o =>
        {
            var confirmed = o.Bookings?.Count(b => b.Status == BookingStatus.Confirmed) ?? 0;
            return new
            {
                o.Id, o.Title, o.Type, o.StartDateTime, o.EndDateTime,
                o.Price, o.Capacity, o.MinEnrollment, o.Status,
                BookedCount = confirmed,
                SpotsLeft = o.Capacity - confirmed,
                AtRisk = confirmed < o.MinEnrollment && o.Status == OfferingStatus.Active,
                GrossIfFull = o.Capacity * o.Price,
                ProjectedGross = confirmed * o.Price,
                PlatformFee = Math.Round(confirmed * o.Price * PlatformFeeRate, 2),
                Trainer = o.TrainerProfile?.DisplayName
            };
        }).ToList();

        return Ok(new
        {
            Offerings = rows,
            Summary = new
            {
                TotalOfferings = rows.Count,
                AtRiskCount = rows.Count(r => r.AtRisk),
                TotalProjectedGross = rows.Sum(r => r.ProjectedGross),
                TotalPlatformFee = rows.Sum(r => r.PlatformFee)
            }
        });
    }

    [HttpGet("pets")]
    public async Task<IActionResult> PetReport([FromQuery] string? breed, CancellationToken ct)
    {
        var pets = await petRepo.GetAllAsync(ct);
        if (!string.IsNullOrWhiteSpace(breed))
            pets = pets.Where(p => p.Breed.Contains(breed, StringComparison.OrdinalIgnoreCase)).ToList();

        var rows = pets.Select(p => new
        {
            p.Id, p.Name, p.Breed,
            p.IsNeutered, p.IsSpayed, p.BirthDate,
            OwnerName = p.Owner != null ? $"{p.Owner.FirstName} {p.Owner.LastName}" : null,
            OwnerEmail = p.Owner?.Email,
            BookingCount = p.Bookings?.Count ?? 0
        }).ToList();

        var breedBreakdown = rows.GroupBy(p => p.Breed)
            .Select(g => new { Breed = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count).ToList();

        return Ok(new { Pets = rows, BreedBreakdown = breedBreakdown, Total = rows.Count });
    }
}
