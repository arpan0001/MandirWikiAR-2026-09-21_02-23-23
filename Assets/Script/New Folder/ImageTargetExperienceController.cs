using UnityEngine;
using Vuforia;

public class ImageTargetExperienceController : MonoBehaviour
{
    [Header("Tracking")]
    [SerializeField]
    private ObserverBehaviour imageTarget;

    [Header("Glow")]
    [SerializeField]
    private ParticleSystem glowParticle;

    private bool isTracked;

    public bool IsTracked => isTracked;

    private void Awake()
    {
        if (imageTarget == null)
            imageTarget =
                GetComponentInParent<ObserverBehaviour>();

        if (imageTarget == null)
        {
            Debug.LogError(
                "[ImageTargetExperienceController] " +
                "ObserverBehaviour not found."
            );
        }

        if (glowParticle != null)
            glowParticle.Stop(true);
    }

    private void OnEnable()
    {
        if (imageTarget != null)
        {
            imageTarget.OnTargetStatusChanged +=
                OnTargetStatusChanged;
        }
    }

    private void OnDisable()
    {
        if (imageTarget != null)
        {
            imageTarget.OnTargetStatusChanged -=
                OnTargetStatusChanged;
        }
    }

    private void OnTargetStatusChanged(
        ObserverBehaviour behaviour,
        TargetStatus status)
    {
        bool currentlyTracked =
            status.Status == Status.TRACKED ||
            status.Status == Status.EXTENDED_TRACKED;

        if (currentlyTracked && !isTracked)
        {
            isTracked = true;
            OnImageDetected();
        }
        else if (!currentlyTracked && isTracked)
        {
            isTracked = false;
            OnImageLost();
        }
    }

    private void OnImageDetected()
    {
        Debug.Log(
            "[ImageTargetExperienceController] " +
            "IMAGE TARGET DETECTED"
        );

        StartGlow();
    }

    private void OnImageLost()
    {
        Debug.Log(
            "[ImageTargetExperienceController] " +
            "IMAGE TARGET LOST"
        );

        StopGlow();
    }

    private void StartGlow()
    {
        if (glowParticle == null)
        {
            Debug.LogWarning(
                "[ImageTargetExperienceController] " +
                "Glow Particle is not assigned."
            );

            return;
        }

        glowParticle.Play();
    }

    private void StopGlow()
    {
        if (glowParticle == null)
            return;

        glowParticle.Stop(
            true,
            ParticleSystemStopBehavior.StopEmitting
        );
    }
}