using MCDrag.Data;
using MCDrag.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MCDrag.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    private readonly DragDbContext _db;

    public TestController(DragDbContext db)
    {
        _db = db;
    }

    // Crea una carrera simple en DB
    [HttpPost("race")]
    public async Task<IActionResult> CreateRace()
    {
        var race = new RaceEntity
        {
            State = "Created",
            Mode = "Sportsman"
        };

        _db.Races.Add(race);
        await _db.SaveChangesAsync();

        return Ok(race);
    }

    // Lista las últimas 20 carreras guardadas
    [HttpGet("races")]
    public async Task<IActionResult> GetRaces()
    {
        var races = await _db.Races
            .OrderByDescending(r => r.CreatedAtUtc)
            .Take(20)
            .ToListAsync();

        return Ok(races);
    }
}