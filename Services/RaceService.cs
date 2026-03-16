using MCDrag.Models;

namespace MCDrag.Services;

public class RaceService : IRaceService
{
    private Race _current = new();

    public Race GetCurrent() => _current;

    public void Prepare(RaceMode mode)
    {
        _current = new Race { Mode = mode, State = RaceState.Ready };
    }

    public void Start()
    {
        if (_current.State != RaceState.Ready)
            throw new InvalidOperationException("Race must be Ready to Start.");

        _current.GreenAtUtc = DateTime.UtcNow;
        _current.State = RaceState.Running;

        // limpia datos previos por si acaso
        ResetLane(_current.Lane1);
        ResetLane(_current.Lane2);
        _current.Winner = null;
    }

    public void LaneLaunched(int lane, bool foul)
    {
        EnsureRunning();
        var lr = GetLane(lane);

        if (lr.StartAtUtc != null)
            throw new InvalidOperationException($"Lane {lane} already launched.");

        // false start si sale antes del verde
        lr.FalseStart = _current.GreenAtUtc.HasValue && DateTime.UtcNow < _current.GreenAtUtc.Value;
        lr.StartAtUtc = DateTime.UtcNow;

        if (_current.GreenAtUtc is null)
            throw new InvalidOperationException("GreenAtUtc is null.");

        // Reaction = salida - verde
        lr.ReactionTimeMs = (lr.StartAtUtc.Value - _current.GreenAtUtc.Value).TotalMilliseconds;

        // Si false start, igual dejamos reacción calculada pero marcamos falta
       // if (falseStart)
      //  {
            // opcional: podrías declarar ganador inmediato al otro carril
            // por ahora lo dejamos como flag
      //  }
    }

    public void LaneFinished(int lane)
    {
        EnsureRunning();
        var lr = GetLane(lane);

        if (lr.StartAtUtc is null)
            throw new InvalidOperationException($"Lane {lane} has not launched yet.");

        if (lr.FinishAtUtc != null)
            throw new InvalidOperationException($"Lane {lane} already finished.");

        lr.FinishAtUtc = DateTime.UtcNow;

        // Elapsed = meta - salida
        lr.ElapsedTimeMs = (lr.FinishAtUtc.Value - lr.StartAtUtc.Value).TotalMilliseconds;

        // Si ambos terminaron (o ambos false start), cerramos
        if (IsLaneDone(_current.Lane1) && IsLaneDone(_current.Lane2))
        {
            DecideWinner();
            _current.State = RaceState.Finished;
        }
    }

    public void Reset()
    {
        _current = new Race();
    }

    // ----------------- helpers -----------------

    private void EnsureRunning()
    {
        if (_current.State != RaceState.Running)
            throw new InvalidOperationException("Race is not Running.");
    }

    private static bool IsLaneDone(LaneResult lr)
        => lr.FalseStart || lr.ElapsedTimeMs != null;

    private LaneResult GetLane(int lane)
    {
        if (lane == 1) return _current.Lane1;
        if (lane == 2) return _current.Lane2;
        throw new ArgumentException("Lane must be 1 or 2.");
    }

    private static void ResetLane(LaneResult lr)
    {
        lr.ReactionTimeMs = null;
        lr.ElapsedTimeMs = null;
        lr.FalseStart = false;
        lr.StartAtUtc = null;
        lr.FinishAtUtc = null;
    }

    private void DecideWinner()
    {
        // Reglas simples:
        // - Si uno false start y el otro NO, gana el otro
        // - Si los dos false start: No Winner
        // - Si ambos con tiempo: menor ElapsedTime gana (si empatan, Tie)

        var a = _current.Lane1;
        var b = _current.Lane2;

        if (a.FalseStart && !b.FalseStart) { _current.Winner = "Lane 2"; return; }
        if (b.FalseStart && !a.FalseStart) { _current.Winner = "Lane 1"; return; }
        if (a.FalseStart && b.FalseStart) { _current.Winner = "No Winner"; return; }

        if (a.ElapsedTimeMs is null || b.ElapsedTimeMs is null)
        {
            _current.Winner = "No Winner";
            return;
        }

        var diff = a.ElapsedTimeMs.Value - b.ElapsedTimeMs.Value;
        if (Math.Abs(diff) < 1.0) _current.Winner = "Tie"; // tolerancia 1ms
        else _current.Winner = diff < 0 ? "Lane 1" : "Lane 2";
    }
}