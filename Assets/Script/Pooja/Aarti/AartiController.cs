using UnityEngine;

public class AartiController : MonoBehaviour, IRitualController
{
    [Header("Ritual Identity")]
    [SerializeField] private string ritualId = "aarti";

    [Header("Components")]
    [SerializeField] private AartiMovement movement;
    [SerializeField]
    private AartiAudio audioController;

    public string RitualId => ritualId;

    public bool IsPlaying =>
        movement != null && movement.IsPlaying;

    public void StartRitual()
    {
        if (movement != null)
        {
            movement.Play();
        }

        if (audioController != null)
        {
            audioController.Play();
        }
    }

    public void StopRitual()
    {
        if (movement != null)
        {
            movement.Stop();
        }

        if (audioController != null)
        {
            audioController.Stop();
        }
    }
}