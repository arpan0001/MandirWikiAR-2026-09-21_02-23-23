using UnityEngine;

public class PoojaaUIController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private GameObject poojaButton;

    [SerializeField]
    private GameObject ritualBar;

    public bool IsPoojaAvailable { get; private set; }

    private void Awake()
    {
        HideAll();
    }

    public void ShowPoojaButton()
    {
        IsPoojaAvailable = true;

        if (poojaButton != null)
            poojaButton.SetActive(true);

        Debug.Log("[PoojaUIController] Pooja button enabled.");
    }

    public void ShowRitualBar()
    {
        if (!IsPoojaAvailable)
        {
            Debug.LogWarning(
                "[PoojaUIController] " +
                "Cannot show ritual bar before Pooja."
            );

            return;
        }

        if (ritualBar != null)
            ritualBar.SetActive(true);

        Debug.Log("[PoojaUIController] Ritual bar shown.");
    }

    public void HideAll()
    {
        IsPoojaAvailable = false;

        if (poojaButton != null)
            poojaButton.SetActive(false);

        if (ritualBar != null)
            ritualBar.SetActive(false);
    }
}