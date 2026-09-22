using UnityEngine;

public class PoojaUIController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject ritualBarObject;
    [SerializeField] private RitualBarController ritualBarController;

    [Header("Idol")]
    [SerializeField] private IdolTargetController idolTargetController;

    private void Awake()
    {
        if (ritualBarObject != null)
        {
            ritualBarObject.SetActive(false);
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

    private void OnIdolDetected(
        IdolDefinition idol)
    {
        if (idol == null)
            return;

        if (ritualBarController != null)
        {
            ritualBarController.BuildForIdol(idol);
        }

        if (ritualBarObject != null)
        {
            ritualBarObject.SetActive(true);
        }
    }

    private void OnIdolLost(
        IdolDefinition idol)
    {
        if (ritualBarObject != null)
        {
            ritualBarObject.SetActive(false);
        }
    }
}