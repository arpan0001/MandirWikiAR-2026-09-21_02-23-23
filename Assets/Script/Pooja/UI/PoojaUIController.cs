using UnityEngine;

public class PoojaUIController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject ritualBar;

    [Header("Idol")]
    [SerializeField] private IdolTargetController idolTargetController;

    private void Awake()
    {
        if (ritualBar != null)
        {
            ritualBar.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (idolTargetController == null)
            return;

        idolTargetController.IdolDetected += OnIdolDetected;
        idolTargetController.IdolLost += OnIdolLost;
    }

    private void OnDisable()
    {
        if (idolTargetController == null)
            return;

        idolTargetController.IdolDetected -= OnIdolDetected;
        idolTargetController.IdolLost -= OnIdolLost;
    }

    private void OnIdolDetected(IdolDefinition idol)
    {
        if (ritualBar != null)
        {
            ritualBar.SetActive(true);
        }
    }

    private void OnIdolLost(IdolDefinition idol)
    {
        if (ritualBar != null)
        {
            ritualBar.SetActive(false);
        }
    }
}