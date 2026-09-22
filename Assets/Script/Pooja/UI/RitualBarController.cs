using UnityEngine;

public class RitualBarController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private RitualButtonUI buttonPrefab;

    [Header("Systems")]
    [SerializeField] private RitualRegistry ritualRegistry;

    private void ClearButtons()
    {
        if (buttonContainer == null)
            return;

        for (int i = buttonContainer.childCount - 1;
             i >= 0;
             i--)
        {
            Destroy(buttonContainer.GetChild(i).gameObject);
        }
    }

    public void BuildForIdol(
        IdolDefinition idolDefinition)
    {
        ClearButtons();

        if (idolDefinition == null)
        {
            Debug.LogWarning(
                "[RitualBarController] IdolDefinition is null."
            );

            return;
        }

        if (ritualRegistry == null)
        {
            Debug.LogError(
                "[RitualBarController] RitualRegistry is not assigned."
            );

            return;
        }

        if (buttonPrefab == null)
        {
            Debug.LogError(
                "[RitualBarController] Button prefab is not assigned."
            );

            return;
        }

        if (buttonContainer == null)
        {
            Debug.LogError(
                "[RitualBarController] Button container is not assigned."
            );

            return;
        }

        foreach (RitualDefinition ritual
                 in idolDefinition.Rituals)
        {
            if (ritual == null)
                continue;

            CreateButton(ritual);
        }
    }

    private void CreateButton(
        RitualDefinition ritualDefinition)
    {
        RitualButtonUI button =
            Instantiate(
                buttonPrefab,
                buttonContainer
            );

        button.Initialize(
            ritualDefinition,
            ritualRegistry
        );
    }
}