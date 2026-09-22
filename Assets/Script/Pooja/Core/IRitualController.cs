public interface IRitualController
{
    string RitualId { get; }

    void StartRitual();
    void StopRitual();
}