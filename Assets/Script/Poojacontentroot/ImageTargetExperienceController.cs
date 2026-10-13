
using System.Collections;
using UnityEngine;
using Vuforia;

public class ImageTargetExperienceController : MonoBehaviour
{
    [Header("Tracking")]
    [SerializeField]
    private ObserverBehaviour imageTarget;

    [Header("Deity Configuration")]
    [SerializeField]
    private DeityDefinition deityDefinition;

    [Header("Glow")]
    [SerializeField]
    private ParticleSystem glowParticle;

    [Header("UI")]
    [SerializeField]
    private PoojaaUIController poojaaUIController;

    [Header("Idol Glow")]
    [SerializeField]
    private ParticleSystem idolGlowParticle;

    [Header("Idol Ambient Audio")]
    [SerializeField]
    private AudioSource idolAmbientAudio;

    private bool otherAudioPlaying;

    [Header("Idol")]
    [SerializeField]
    private GameObject idolRoot;

    [SerializeField]
    private Animator idolAnimator;

    [Header("Timing")]
    [SerializeField]
    private float glowDelay = 0.8f;

    private bool isTracked;
    private bool experienceStarted;
    private bool revealFinished;

    private Coroutine revealCoroutine;
    private Coroutine revealCompletionCoroutine;

    private static readonly int RevealHash =
        Animator.StringToHash("Reveal");

    public bool IsTracked => isTracked;
    public DeityDefinition CurrentDeity => deityDefinition;

    private void Awake()
    {
        // Find the Vuforia Image Target.
        if (imageTarget == null)
        {
            imageTarget =
                GetComponentInParent<ObserverBehaviour>();
        }

        if (imageTarget == null)
        {
            Debug.LogError(
                "[ImageTargetExperienceController] " +
                "ObserverBehaviour not found.",
                this
            );
        }

        // Configure deity-specific ambient audio.
        ConfigureDeity();

        // Stop the initial scan glow.
        if (glowParticle != null)
        {
            glowParticle.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );
        }

        // Hide the idol until the reveal sequence begins.
        if (idolRoot != null)
        {
            idolRoot.SetActive(false);
        }

        // Hide the Pooja UI at startup.
        if (poojaaUIController != null)
        {
            poojaaUIController.HideAll();
        }
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

        StopRevealCoroutines();
    }

    private void ConfigureDeity()
    {
        if (deityDefinition == null)
        {
            Debug.LogWarning(
                "[ImageTargetExperienceController] " +
                "No DeityDefinition assigned. " +
                "Existing Inspector audio settings will be retained.",
                this
            );

            return;
        }

        if (idolAmbientAudio != null)
        {
            if (deityDefinition.AmbientAudio != null)
            {
                idolAmbientAudio.clip =
                    deityDefinition.AmbientAudio;
            }

            idolAmbientAudio.loop = true;
        }

        Debug.Log(
            "[ImageTargetExperienceController] Configured deity: " +
            deityDefinition.DisplayName,
            this
        );
    }

    private void OnTargetStatusChanged(
        ObserverBehaviour behaviour,
        TargetStatus status)
    {
        bool currentlyTracked =
            status.Status == Status.TRACKED ||
            status.Status == Status.EXTENDED_TRACKED;

        // Image Target detected.
        if (currentlyTracked && !isTracked)
        {
            isTracked = true;

            Debug.Log(
                "[ImageTargetExperienceController] " +
                "IMAGE TARGET DETECTED",
                this
            );

            StartExperience();
        }
        // Image Target lost.
        else if (!currentlyTracked && isTracked)
        {
            isTracked = false;

            Debug.Log(
                "[ImageTargetExperienceController] " +
                "IMAGE TARGET LOST",
                this
            );

            ResetExperience();
        }
    }

    private void StartExperience()
    {
        if (experienceStarted)
        {
            return;
        }

        experienceStarted = true;
        revealFinished = false;

        revealCoroutine = StartCoroutine(RevealSequence());
    }

    public void SetOtherAudioPlaying(bool isPlaying)
    {
        otherAudioPlaying = isPlaying;

        if (isPlaying)
        {
            StopIdolAmbientAudio();
        }
        else
        {
            StartIdolAmbientAudio();
        }
    }

    private void StartIdolAmbientAudio()
    {
        if (!isTracked || otherAudioPlaying)
        {
            return;
        }

        if (idolAmbientAudio == null)
        {
            return;
        }

        if (idolAmbientAudio.clip == null)
        {
            Debug.LogWarning(
                "[ImageTargetExperienceController] " +
                "Ambient AudioSource has no clip.",
                this
            );

            return;
        }

        if (idolAmbientAudio.isPlaying)
        {
            return;
        }

        idolAmbientAudio.loop = true;
        idolAmbientAudio.Play();

        Debug.Log(
            "[ImageTargetExperienceController] " +
            "Idol ambient audio resumed.",
            this
        );
    }

    private void StopIdolAmbientAudio()
    {
        if (idolAmbientAudio != null &&
            idolAmbientAudio.isPlaying)
        {
            idolAmbientAudio.Stop();

            Debug.Log(
                "[ImageTargetExperienceController] " +
                "Idol ambient audio stopped.",
                this
            );
        }
    }

    private IEnumerator RevealSequence()
    {
        Debug.Log(
            "[ImageTargetExperienceController] " +
            "Starting reveal sequence.",
            this
        );

        // STEP 1: Start the initial glow.
        if (glowParticle != null)
        {
            glowParticle.Play();

            Debug.Log(
                "[ImageTargetExperienceController] " +
                "Glow started.",
                this
            );
        }

        // STEP 2: Wait for the glow.
        yield return new WaitForSeconds(glowDelay);

        // Stop if tracking was lost during the delay.
        if (!isTracked)
        {
            revealCoroutine = null;
            yield break;
        }

        // STEP 3: Show the deity model.
        if (idolRoot != null)
        {
            idolRoot.SetActive(true);

            Debug.Log(
                "[ImageTargetExperienceController] " +
                "Idol enabled.",
                this
            );
        }
        else
        {
            Debug.LogError(
                "[ImageTargetExperienceController] " +
                "Idol Root is not assigned.",
                this
            );

            revealCoroutine = null;
            yield break;
        }

        // Start the deity glow.
        if (idolGlowParticle != null)
        {
            idolGlowParticle.Play();

            Debug.Log(
                "[ImageTargetExperienceController] " +
                "Idol glow started.",
                this
            );
        }

        // Start ambient audio unless another ritual owns audio.
        if (!otherAudioPlaying)
        {
            StartIdolAmbientAudio();
        }

        // STEP 4: Play the reveal animation.
        if (idolAnimator != null)
        {
            idolAnimator.ResetTrigger(RevealHash);
            idolAnimator.SetTrigger(RevealHash);

            Debug.Log(
                "[ImageTargetExperienceController] " +
                "Deity reveal animation started.",
                this
            );

            revealCompletionCoroutine =
                StartCoroutine(WaitForRevealToFinish());
        }
        else
        {
            Debug.LogWarning(
                "[ImageTargetExperienceController] " +
                "Idol Animator is not assigned.",
                this
            );
        }

        revealCoroutine = null;
    }

    private IEnumerator WaitForRevealToFinish()
    {
        // Give the Animator time to enter its reveal state.
        yield return null;

        // Wait until the reveal state has finished.
        while (isTracked && idolAnimator != null)
        {
            AnimatorStateInfo stateInfo =
                idolAnimator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("HanumanReveal") &&
                stateInfo.normalizedTime >= 1f)
            {
                break;
            }

            yield return null;
        }

        revealCompletionCoroutine = null;

        if (!isTracked)
        {
            yield break;
        }

        revealFinished = true;

        Debug.Log(
            "[ImageTargetExperienceController] " +
            "Deity reveal completed.",
            this
        );

        // Show the Pooja button after the reveal.
        if (poojaaUIController != null)
        {
            poojaaUIController.ShowPoojaButton();

            Debug.Log(
                "[ImageTargetExperienceController] " +
                "Pooja button enabled.",
                this
            );
        }
        else
        {
            Debug.LogWarning(
                "[ImageTargetExperienceController] " +
                "PoojaaUIController is not assigned.",
                this
            );
        }
    }

    private void ResetExperience()
    {
        StopRevealCoroutines();

        // Reset state.
        experienceStarted = false;
        revealFinished = false;
        otherAudioPlaying = false;

        // Stop the initial glow.
        if (glowParticle != null)
        {
            glowParticle.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );
        }

        // Stop deity glow.
        if (idolGlowParticle != null)
        {
            idolGlowParticle.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );
        }

        // Stop ambient audio.
        StopIdolAmbientAudio();

        // Reset the Animator.
        if (idolAnimator != null)
        {
            idolAnimator.ResetTrigger(RevealHash);
            idolAnimator.Play("Idle", 0, 0f);
            idolAnimator.Update(0f);
        }

        // Hide the idol.
        if (idolRoot != null)
        {
            idolRoot.SetActive(false);
        }

        // Reset the UI.
        if (poojaaUIController != null)
        {
            poojaaUIController.HideAll();
        }

        Debug.Log(
            "[ImageTargetExperienceController] " +
            "Experience reset.",
            this
        );
    }

    private void StopRevealCoroutines()
    {
        if (revealCoroutine != null)
        {
            StopCoroutine(revealCoroutine);
            revealCoroutine = null;
        }

        if (revealCompletionCoroutine != null)
        {
            StopCoroutine(revealCompletionCoroutine);
            revealCompletionCoroutine = null;
        }
    }
}
