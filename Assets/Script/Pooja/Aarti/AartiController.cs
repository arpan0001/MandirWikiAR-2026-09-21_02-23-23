using UnityEngine;

public class AartiController : MonoBehaviour, IRitualController
{
    [Header("Ritual")]
    [SerializeField] private string ritualId = "aarti";

    [Header("Movement")]
    [SerializeField] private AartiDOTweenMovement movement;

    [Header("Audio")]
    [SerializeField] private AartiAudio audio;

    public string RitualId => ritualId;

    public bool IsPlaying =>
        movement != null &&
        movement.IsPlaying;

    private void Awake()
    {
        if (movement == null)
        {
            movement =
                GetComponentInChildren<AartiDOTweenMovement>();
        }
    }

    public void StartRitual()
    {
        if (movement != null)
        {
            movement.Play();
        }

        if (audio != null)
        {
            audio.Play();
        }
    }

    public void StopRitual()
    {
        if (movement != null)
        {
            movement.Stop();
        }

        if (audio != null)
        {
            audio.Stop();
        }
    }
}