using LTS.Core;
using Microsoft.AspNetCore.Mvc;

namespace LTS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PetsController(IPetRepository repo) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok((await repo.GetAllAsync(ct)).Select(Map));

    [HttpGet("owner/{ownerId:int}")]
    public async Task<IActionResult> GetByOwner(int ownerId, CancellationToken ct) =>
        Ok((await repo.GetByOwnerAsync(ownerId, ct)).Select(Map));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var pet = await repo.GetByIdAsync(id, ct);
        return pet == null ? NotFound() : Ok(Map(pet));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePetRequest req, CancellationToken ct)
    {
        var pet = new Pet
        {
            OwnerId = req.OwnerId,
            Name = req.Name,
            Breed = req.Breed,
            IsNeutered = req.IsNeutered,
            IsSpayed = req.IsSpayed,
            BirthDate = req.BirthDate,
            Notes = req.Notes
        };
        await repo.AddAsync(pet, ct);
        return CreatedAtAction(nameof(GetById), new { id = pet.Id }, Map(pet));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePetRequest req, CancellationToken ct)
    {
        var pet = await repo.GetByIdAsync(id, ct);
        if (pet == null) return NotFound();
        pet.Name = req.Name;
        pet.Breed = req.Breed;
        pet.IsNeutered = req.IsNeutered;
        pet.IsSpayed = req.IsSpayed;
        pet.BirthDate = req.BirthDate;
        pet.Notes = req.Notes;
        await repo.UpdateAsync(pet, ct);
        return Ok(Map(pet));
    }

    static object Map(Pet p) => new
    {
        p.Id, p.OwnerId, p.Name, p.Breed,
        p.IsNeutered, p.IsSpayed, p.BirthDate, p.Notes,
        OwnerName = p.Owner != null ? $"{p.Owner.FirstName} {p.Owner.LastName}" : null
    };
}

public record CreatePetRequest(int OwnerId, string Name, string Breed, bool IsNeutered, bool IsSpayed, DateTime? BirthDate, string? Notes);
public record UpdatePetRequest(string Name, string Breed, bool IsNeutered, bool IsSpayed, DateTime? BirthDate, string? Notes);
