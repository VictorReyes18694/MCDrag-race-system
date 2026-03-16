namespace MCDrag.Models;

public class LaneResult
{
    public int Lane { get; set; }

    // Tiempos calculados por backend
    public double? ReactionTimeMs { get; set; }   // salida - verde
    public double? ElapsedTimeMs { get; set; }    // meta - salida

    public bool FalseStart { get; set; }

    // Marcas internas
    public DateTime? StartAtUtc { get; set; }     // cuando el auto salió
    public DateTime? FinishAtUtc { get; set; }    // cuando cruzó meta
}