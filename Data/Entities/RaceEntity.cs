using System.Collections.Generic;

namespace MCDrag.Data.Entities
{
    public class RaceEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public string State { get; set; } = "Created";
        public string Mode { get; set; } = "Sportsman";
        public List<LaneResultEntity> LaneResults { get; set; } = new();
        public DateTime? GreenAtUtc { get; set; }
        public DateTime? FinishedAtUtc { get; set; }
        public string? Winner { get; set; }
    }
}