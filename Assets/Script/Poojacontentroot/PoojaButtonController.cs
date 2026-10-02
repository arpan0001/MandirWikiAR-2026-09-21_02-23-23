using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PoojaButtonController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Button button;

    [SerializeField]
    private PoojaContentController poojaContentController;

    [SerializeField]
    private PoojaaUIController poojaaUIController;

    [SerializeField]
    private TMP_Text buttonText;

    [Header("Labels")]
    [SerializeField]
    private string startLabel = "Pooja";

    [SerializeField]
    private string startedLabel = "Pooja Started";

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (button != null)
            button.onClick.AddListener(
                OnButtonClicked
            );
    }

    private void Start()
    {
        UpdateVisual();
    }

    private void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(
                OnButtonClicked
            );
    }

    private void OnButtonClicked()
    {
        if (poojaContentController == null)
        {
            Debug.LogWarning(
                "[PoojaButtonController] " +
                "PoojaContentController is not assigned."
            );

            return;
        }

        if (poojaContentController.PoojaStarted)
            return;

        poojaContentController.StartPooja();

        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (buttonText == null)
            return;

        if (poojaContentController != null &&
            poojaContentController.PoojaStarted)
        {
            buttonText.text = startedLabel;
        }
        else
        {
            buttonText.text = startLabel;
        }
    }
}