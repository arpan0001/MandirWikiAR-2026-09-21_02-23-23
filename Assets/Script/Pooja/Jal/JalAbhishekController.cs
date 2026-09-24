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
    private JalAbhishekAudio audioController;

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

        if (audioController != null)
        {
            audioController.Play();
        }
    }

    public void StopRitual()
    {
        if (system != null)
        {
            system.Stop();
        }

        if (audioController != null)
        {
            audioController.Stop();
        }
    }
}