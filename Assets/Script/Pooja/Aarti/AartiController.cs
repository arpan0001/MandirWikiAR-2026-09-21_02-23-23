using UnityEngine;

public class AartiController : MonoBehaviour, IRitualController
{
    [Header("Components")]
    [SerializeField] private AartiMovement movement;
    [SerializeField] private AartiAudio audio;

    public bool IsPlaying => movement != null && movement.IsPlaying;

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