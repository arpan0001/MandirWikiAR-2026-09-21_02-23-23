using System.Collections;
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

    [Header("UI")]
    [SerializeField]
    private PoojaaUIController poojaaUIController;

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

    private void Awake()
    {
        // --------------------------------
        // Find Vuforia Image Target
        // --------------------------------

        if (imageTarget == null)
        {
            imageTarget =
                GetComponentInParent<ObserverBehaviour>();
        }

        if (imageTarget == null)
        {
            Debug.LogError(
                "[ImageTargetExperienceController] " +
                "ObserverBehaviour not found."
            );
        }

        // --------------------------------
        // Stop Glow at Startup
        // --------------------------------

        if (glowParticle != null)
        {
            glowParticle.Stop(true);
        }

        // --------------------------------
        // Hide Idol at Startup
        // --------------------------------

        if (idolRoot != null)
        {
            idolRoot.SetActive(false);
        }

        // --------------------------------
        // Hide UI at Startup
        // --------------------------------

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
    }

    private void OnTargetStatusChanged(
        ObserverBehaviour behaviour,
        TargetStatus status)
    {
        bool currentlyTracked =
            status.Status == Status.TRACKED ||
            status.Status == Status.EXTENDED_TRACKED;

        // --------------------------------
        // IMAGE TARGET DETECTED
        // --------------------------------

        if (currentlyTracked && !isTracked)
        {
            isTracked = true;

            Debug.Log(
                "[ImageTargetExperienceController] " +
                "IMAGE TARGET DETECTED"
            );

            StartExperience();
        }

        // --------------------------------
        // IMAGE TARGET LOST
        // --------------------------------

        else if (!currentlyTracked && isTracked)
        {
            isTracked = false;

            Debug.Log(
                "[ImageTargetExperienceController] " +
                "IMAGE TARGET LOST"
            );

            ResetExperience();
        }
    }

    private void StartExperience()
    {
        if (experienceStarted)
            return;

        experienceStarted = true;
        revealFinished = false;

        revealCoroutine =
            StartCoroutine(
                RevealSequence()
            );
    }

    private IEnumerator RevealSequence()
    {
        Debug.Log(
            "[ImageTargetExperienceController] " +
            "Starting reveal sequence."
        );

        // ========================================
        // STEP 1: START GLOW
        // ========================================

        if (glowParticle != null)
        {
            glowParticle.Play();

            Debug.Log(
                "[ImageTargetExperienceController] " +
                "Glow started."
            );
        }

        // ========================================
        // STEP 2: WAIT FOR GLOW
        // ========================================

        yield return new WaitForSeconds(
            glowDelay
        );

        // ========================================
        // CHECK IF IMAGE IS STILL TRACKED
        // ========================================

        if (!isTracked)
        {
            yield break;
        }

        // ========================================
        // STEP 3: SHOW IDOL
        // ========================================

        if (idolRoot != null)
        {
            idolRoot.SetActive(true);

            Debug.Log(
                "[ImageTargetExperienceController] " +
                "Idol enabled."
            );
        }

        // ========================================
        // STEP 4: PLAY HANUMAN REVEAL
        // ========================================

        if (idolAnimator != null)
        {
            idolAnimator.ResetTrigger(
                RevealHash
            );

            idolAnimator.SetTrigger(
                RevealHash
            );

            Debug.Log(
                "[ImageTargetExperienceController] " +
                "Hanuman reveal animation started."
            );

            // Wait for animation to finish
            revealCompletionCoroutine =
                StartCoroutine(
                    WaitForRevealToFinish()
                );
        }
        else
        {
            Debug.LogWarning(
                "[ImageTargetExperienceController] " +
                "Idol Animator is not assigned."
            );
        }

        revealCoroutine = null;
    }

    private IEnumerator WaitForRevealToFinish()
    {
        // Give Animator one frame to enter
        // the HanumanReveal state.
        yield return null;

        while (isTracked && idolAnimator != null)
        {
            AnimatorStateInfo stateInfo =
                idolAnimator.GetCurrentAnimatorStateInfo(0);

            // Check that Animator has entered
            // the correct reveal state.
            if (stateInfo.IsName("HanumanReveal"))
            {
                // normalizedTime:
                //
                // 0 = animation just started
                // 0.5 = halfway
                // 1 = animation completed

                if (stateInfo.normalizedTime >= 1f)
                {
                    break;
                }
            }

            yield return null;
        }

        revealCompletionCoroutine = null;

        // --------------------------------
        // Make sure image is still tracked
        // --------------------------------

        if (!isTracked)
            yield break;

        // --------------------------------
        // Reveal completed
        // --------------------------------

        revealFinished = true;

        Debug.Log(
            "[ImageTargetExperienceController] " +
            "HANUMAN REVEAL COMPLETED."
        );

        // --------------------------------
        // Show Pooja Button
        // --------------------------------

        if (poojaaUIController != null)
        {
            poojaaUIController.ShowPoojaButton();

            Debug.Log(
                "[ImageTargetExperienceController] " +
                "POOJA BUTTON ENABLED."
            );
        }
        else
        {
            Debug.LogWarning(
                "[ImageTargetExperienceController] " +
                "PoojaaUIController is not assigned."
            );
        }
    }

    private void ResetExperience()
    {
        // ========================================
        // STOP REVEAL COROUTINE
        // ========================================

        if (revealCoroutine != null)
        {
            StopCoroutine(
                revealCoroutine
            );

            revealCoroutine = null;
        }

        if (revealCompletionCoroutine != null)
        {
            StopCoroutine(
                revealCompletionCoroutine
            );

            revealCompletionCoroutine = null;
        }

        // ========================================
        // RESET STATE
        // ========================================

        experienceStarted = false;
        revealFinished = false;

        // ========================================
        // STOP GLOW
        // ========================================

        if (glowParticle != null)
        {
            glowParticle.Stop(
                true,
                ParticleSystemStopBehavior.StopEmitting
            );
        }

        // ========================================
        // RESET HANUMAN ANIMATOR
        // ========================================

        if (idolAnimator != null)
        {
            idolAnimator.ResetTrigger(
                RevealHash
            );

            idolAnimator.Play(
                "Idle",
                0,
                0f
            );

            idolAnimator.Update(0f);
        }

        // ========================================
        // HIDE IDOL
        // ========================================

        if (idolRoot != null)
        {
            idolRoot.SetActive(false);
        }

        // ========================================
        // RESET UI
        // ========================================

        if (poojaaUIController != null)
        {
            poojaaUIController.HideAll();
        }

        Debug.Log(
            "[ImageTargetExperienceController] " +
            "Experience reset."
        );
    }
}