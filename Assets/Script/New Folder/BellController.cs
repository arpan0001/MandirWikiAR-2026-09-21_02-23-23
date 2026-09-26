using UnityEngine;

public class BellController : MonoBehaviour
{
    [Header("Ritual")]
    [SerializeField]
    private string ritualId = "bell";

    [Header("Animation")]
    [SerializeField]
    private Animator animator;

    [Header("Audio")]
    [SerializeField]
    private AudioSource audioSource;

    [Header("Settings")]
    [SerializeField]
    private string animationTrigger = "Ring";

    public string RitualId => ritualId;

    public bool IsPlaying { get; private set; }

    private static readonly int RingHash =
        Animator.StringToHash("Ring");

    public void StartRitual()
    {
        if (IsPlaying)
            return;

        IsPlaying = true;

        if (animator != null)
        {
            animator.ResetTrigger(RingHash);
            animator.SetTrigger(RingHash);
        }

        if (audioSource != null &&
            audioSource.clip != null)
        {
            audioSource.Play();
        }

        Debug.Log(
            "[BellController] Bell started."
        );
    }

    public void StopRitual()
    {
        if (!IsPlaying)
            return;

        IsPlaying = false;

        if (audioSource != null)
            audioSource.Stop();

        Debug.Log(
            "[BellController] Bell stopped."
        );
    }
}