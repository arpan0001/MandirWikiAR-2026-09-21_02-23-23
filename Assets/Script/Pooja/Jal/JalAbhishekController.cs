using UnityEngine;

public class JalAbhishekController : MonoBehaviour, IRitualController
{
    [Header("Ritual")]
    [SerializeField]
    private string ritualId = "jal_abhishek";

    [Header("Components")]
    [SerializeField]
    private JalAbhishekSystem system;

    [SerializeField]
    private JalAbhishekAudio audio;

    public string RitualId => ritualId;

    public bool IsPlaying =>
        system != null &&
        system.IsPlaying;

    public void StartRitual()
    {
        if (system == null)
        {
            Debug.LogError(
                "[JalAbhishekController] " +
                "JalAbhishekSystem is not assigned."
            );

            return;
        }

        system.Play();

        if (audio != null)
        {
            audio.Play();
        }
    }

    public void StopRitual()
    {
        if (system != null)
        {
            system.Stop();
        }

        if (audio != null)
        {
            audio.Stop();
        }
    }
}