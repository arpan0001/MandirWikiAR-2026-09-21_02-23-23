
using UnityEngine;

public class DeityExperienceController : MonoBehaviour
{
    [Header("Deity Configuration")]
    [SerializeField] private DeityDefinition definition;

    [Header("Existing Scene Objects")]
    [SerializeField] private Animator deityAnimator;
    [SerializeField] private GameObject glowVFXRoot;
    [SerializeField] private AudioSource ambientAudioSource;
    [SerializeField] private AudioSource aartiAudioSource;

    [Header("Reveal Settings")]
    [SerializeField] private bool triggerRevealOnActivate = false;
    [SerializeField] private string revealTriggerName = "Reveal";

    private int revealTriggerHash;
    private bool isConfigured;
    private bool isActive;

    public DeityDefinition Definition => definition;
    public bool IsActive => isActive;

    private void Awake()
    {
        revealTriggerHash = Animator.StringToHash(revealTriggerName);
    }

    private void Start()
    {
        Configure();
    }

    public void Configure()
    {
        if (definition == null)
        {
            Debug.LogError(
                "[DeityExperienceController] No DeityDefinition assigned.",
                this
            );
            isConfigured = false;
            return;
        }

        if (ambientAudioSource != null)
        {
            ambientAudioSource.clip = definition.AmbientAudio;
            ambientAudioSource.loop = true;
        }

        if (aartiAudioSource != null)
        {
            aartiAudioSource.clip = definition.AartiAudio;
        }

        if (glowVFXRoot != null)
        {
            glowVFXRoot.SetActive(false);
        }

        isConfigured = true;

        Debug.Log(
            $"[DeityExperienceController] Configured: {definition.DisplayName}",
            this
        );
    }

    public void ActivateExperience()
    {
        if (!isConfigured)
        {
            Configure();
        }

        if (!isConfigured || isActive)
            return;

        isActive = true;

        if (glowVFXRoot != null)
        {
            glowVFXRoot.SetActive(true);
        }

        if (triggerRevealOnActivate && deityAnimator != null)
        {
            if (HasAnimatorTrigger(deityAnimator, revealTriggerName))
            {
                deityAnimator.SetTrigger(revealTriggerHash);
            }
            else
            {
                Debug.LogWarning(
                    $"Animator trigger '{revealTriggerName}' was not found.",
                    deityAnimator
                );
            }
        }

        if (ambientAudioSource != null &&
            definition.AmbientAudio != null)
        {
            ambientAudioSource.Play();
        }

        Debug.Log(
            $"[DeityExperienceController] Activated: {definition.DisplayName}",
            this
        );
    }

    public void DeactivateExperience()
    {
        if (!isActive)
            return;

        isActive = false;

        if (ambientAudioSource != null)
        {
            ambientAudioSource.Stop();
        }

        if (glowVFXRoot != null)
        {
            glowVFXRoot.SetActive(false);
        }

        Debug.Log(
            "[DeityExperienceController] Deactivated.",
            this
        );
    }

    private bool HasAnimatorTrigger(Animator animator, string triggerName)
    {
        foreach (AnimatorControllerParameter parameter
                 in animator.parameters)
        {
            if (parameter.name == triggerName &&
                parameter.type == AnimatorControllerParameterType.Trigger)
            {
                return true;
            }
        }

        return false;
    }
}
