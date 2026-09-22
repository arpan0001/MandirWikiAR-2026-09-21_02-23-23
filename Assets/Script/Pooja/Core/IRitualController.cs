public interface IRitualController
{
    string RitualId { get; }
    bool IsPlaying { get; }
    void StartRitual();
    void StopRitual();
}