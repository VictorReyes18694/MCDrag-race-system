using System;

namespace MCDrag.Data.Entities
{
    public class LaneResultEntity
    {
        public Guid Id { get; set; }
        public Guid RaceId { get; set; }
        public int Lane { get; set; } // 1 o 2

        public bool FalseStart { get; set; }
        public DateTime? StartAtUtc { get; set; }
        public DateTime? FinishAtUtc { get; set; }
        public double? ReactionTimeMs { get; set; }
        public double? ElapsedTimeMs { get; set; }

        public RaceEntity? Race { get; set; }
    }
}