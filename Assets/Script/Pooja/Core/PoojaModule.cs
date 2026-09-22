using UnityEngine;

public class PoojaModule : MonoBehaviour
{
    [SerializeField]
    private RitualAnchor ritualAnchor;

    private IdolDefinition idolDefinition;

    public IdolDefinition IdolDefinition => idolDefinition;
    public RitualAnchor RitualAnchor => ritualAnchor;
    public bool IsInitialized => idolDefinition != null;

    public void Initialize(IdolDefinition definition)
    {
        if (definition == null)
        {
            Debug.LogError(
                "[PoojaModule] Cannot initialize with a null IdolDefinition."
            );

            return;
        }

        idolDefinition = definition;

        Debug.Log(
            $"[PoojaModule] Initialized for idol: " +
            $"{idolDefinition.DisplayName}"
        );
    }
}