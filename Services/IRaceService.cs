using MCDrag.Models;

namespace MCDrag.Services;

public interface IRaceService
{
    Race GetCurrent();
    void Prepare(RaceMode mode);
    void Start();                     // setea GreenAtUtc y pone Running
    void LaneLaunched(int lane, bool falseStart = false); // auto salió
    void LaneFinished(int lane);      // auto cruzó meta
    void Reset();
}