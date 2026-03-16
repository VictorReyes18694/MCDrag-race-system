using MCDrag.Models;
using MCDrag.Services;
using Microsoft.AspNetCore.Mvc;

namespace MCDrag.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RaceController : ControllerBase
{
    private readonly IRaceService _raceService;

    public RaceController(IRaceService raceService)
    {
        _raceService = raceService;
    }

    [HttpGet("current")]
    public ActionResult<Race> GetCurrent() => Ok(_raceService.GetCurrent());

    [HttpPost("prepare/{mode}")]
    public IActionResult Prepare(RaceMode mode)
    {
        _raceService.Prepare(mode);
        return Ok(_raceService.GetCurrent());
    }

    [HttpPost("start")]
    public IActionResult Start()
    {
        _raceService.Start();
        return Ok(_raceService.GetCurrent());
    }

    // ESP32 llama esto cuando detecta salida (sensor inicio)
    [HttpPost("lane/{lane}/launch")]
    public IActionResult Launch(int lane)
    {
    _raceService.LaneLaunched(lane);
    return Ok(_raceService.GetCurrent());
    }

    // ESP32 llama esto cuando detecta meta (sensor final)
    [HttpPost("lane/{lane}/finish")]
    public IActionResult Finish(int lane)
    {
        _raceService.LaneFinished(lane);
        return Ok(_raceService.GetCurrent());
    }

    [HttpPost("reset")]
    public IActionResult Reset()
    {
        _raceService.Reset();
        return Ok();
    }
}