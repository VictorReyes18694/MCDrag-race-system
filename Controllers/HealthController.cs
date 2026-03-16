using MCDrag.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MCDrag.Controllers
{
    [ApiController]
    [Route("api/health")]
    public class HealthController : ControllerBase
    {
        private readonly DragDbContext _db;

        public HealthController(DragDbContext db)
        {
            _db = db;
        }

        [HttpGet("db")]
        public async Task<IActionResult> CheckDb()
        {
            try
            {
                var canConnect = await _db.Database.CanConnectAsync();
                return Ok(new { database = canConnect ? "OK" : "FAIL" });
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    title: "Database connection error"
                );
            }
        }
    }
}