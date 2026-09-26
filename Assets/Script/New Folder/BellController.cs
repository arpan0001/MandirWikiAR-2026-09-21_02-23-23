using UnityEngine;

public class BellController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private AudioSource audioSource;

    private static readonly int RingHash =
        Animator.StringToHash("Ring");

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void Ring()
    {
        if (animator == null)
        {
            Debug.LogError(
                "[BellController] " +
                "Animator is missing."
            );

            return;
        }

        animator.ResetTrigger(RingHash);
        animator.SetTrigger(RingHash);

        if (audioSource != null &&
            audioSource.clip != null)
        {
            audioSource.Play();
        }

        Debug.Log(
            "[BellController] Bell Ring triggered."
        );
    }

    public void StopAudio()
    {
        if (audioSource != null)
            audioSource.Stop();
    }
}