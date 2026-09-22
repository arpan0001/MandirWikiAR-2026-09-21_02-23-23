using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RitualButtonUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text label;
    [SerializeField] private Image icon;

    private RitualDefinition ritualDefinition;
    private RitualRegistry ritualRegistry;

    private bool isPlaying;

    public void Initialize(
        RitualDefinition definition,
        RitualRegistry registry)
    {
        if (definition == null)
        {
            Debug.LogError(
                "[RitualButtonUI] RitualDefinition is null."
            );

            return;
        }

        if (registry == null)
        {
            Debug.LogError(
                "[RitualButtonUI] RitualRegistry is null."
            );

            return;
        }

        ritualDefinition = definition;
        ritualRegistry = registry;

        SetupVisuals();

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClicked);
        }
    }

    private void SetupVisuals()
    {
        if (label != null)
        {
            label.text = ritualDefinition.DisplayName;
        }

        if (icon != null)
        {
            icon.sprite = ritualDefinition.Icon;
            icon.enabled = ritualDefinition.Icon != null;
        }
    }

    private void OnClicked()
    {
        if (ritualDefinition == null)
            return;

        if (ritualRegistry == null)
            return;

        if (!ritualRegistry.TryGetRitual(
                ritualDefinition.Id,
                out IRitualController controller))
        {
            Debug.LogWarning(
                $"[RitualButtonUI] No controller found for " +
                $"ritual: {ritualDefinition.Id}"
            );

            return;
        }

        if (isPlaying)
        {
            controller.StopRitual();
            isPlaying = false;
        }
        else
        {
            controller.StartRitual();
            isPlaying = true;
        }

        UpdateButtonLabel();
    }

    private void UpdateButtonLabel()
    {
        if (label == null)
            return;

        if (isPlaying)
        {
            label.text =
                $"Stop {ritualDefinition.DisplayName}";
        }
        else
        {
            label.text =
                ritualDefinition.DisplayName;
        }
    }
}