namespace MCDrag.Models;

//public enum RaceState { Idle, Ready, Running, Finished }
//public enum RaceMode { Sportsman } // lo puedes expandir después

public class Race
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public RaceMode Mode { get; set; } = RaceMode.Sportsman;
    public RaceState State { get; set; } = RaceState.Idle;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    // Momento “verde” (cuando se da la salida)
    public DateTime? GreenAtUtc { get; set; }

    public LaneResult Lane1 { get; set; } = new() { Lane = 1 };
    public LaneResult Lane2 { get; set; } = new() { Lane = 2 };

    // Resultado
    public string? Winner { get; set; } // "Lane 1", "Lane 2", "Tie", "No Winner"
}