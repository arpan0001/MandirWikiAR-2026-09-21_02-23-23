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
    private RitualManager ritualManager;

    public void Initialize(
        RitualDefinition definition,
        RitualManager manager)
    {
        if (definition == null)
        {
            Debug.LogError(
                "[RitualButtonUI] RitualDefinition is null."
            );

            return;
        }

        if (manager == null)
        {
            Debug.LogError(
                "[RitualButtonUI] RitualManager is null."
            );

            return;
        }

        ritualDefinition = definition;
        ritualManager = manager;

        SetupVisuals();

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClicked);
        }

        ritualManager.RitualStarted += OnRitualStarted;
        ritualManager.RitualStopped += OnRitualStopped;

        UpdateVisualState();
    }

    private void SetupVisuals()
    {
        if (label != null)
        {
            label.text =
                ritualDefinition.DisplayName;
        }

        if (icon != null)
        {
            icon.sprite =
                ritualDefinition.Icon;

            icon.enabled =
                ritualDefinition.Icon != null;
        }
    }

    private void OnClicked()
    {
        if (ritualManager == null ||
            ritualDefinition == null)
        {
            return;
        }

        if (ritualManager.IsActive(
                ritualDefinition))
        {
            ritualManager.StopRitual(
                ritualDefinition
            );
        }
        else
        {
            ritualManager.StartRitual(
                ritualDefinition
            );
        }
    }

    private void OnRitualStarted(
        RitualDefinition startedRitual)
    {
        UpdateVisualState();
    }

    private void OnRitualStopped(
        RitualDefinition stoppedRitual)
    {
        UpdateVisualState();
    }

    private void UpdateVisualState()
    {
        if (label == null ||
            ritualDefinition == null)
        {
            return;
        }

        bool isActive =
            ritualManager != null &&
            ritualManager.IsActive(
                ritualDefinition
            );

        if (isActive)
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

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(
                OnClicked
            );
        }

        if (ritualManager != null)
        {
            ritualManager.RitualStarted -=
                OnRitualStarted;

            ritualManager.RitualStopped -=
                OnRitualStopped;
        }
    }
}