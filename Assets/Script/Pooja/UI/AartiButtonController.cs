using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AartiButtonController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button button;
    [SerializeField] private AartiController aartiController;
    [SerializeField] private TMP_Text buttonText;

    [Header("Labels")]
    [SerializeField] private string startLabel = "Aarti";
    [SerializeField] private string stopLabel = "Stop Aarti";

    private void Awake()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }

        if (button != null)
        {
            button.onClick.AddListener(OnButtonClicked);
        }
    }

    private void Start()
    {
        UpdateVisualState();
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(OnButtonClicked);
        }
    }

    private void OnButtonClicked()
    {
        if (aartiController == null)
        {
            Debug.LogWarning(
                "[AartiButtonController] " +
                "AartiController is not assigned."
            );

            return;
        }

        if (aartiController.IsPlaying)
        {
            aartiController.StopRitual();
        }
        else
        {
            aartiController.StartRitual();
        }

        UpdateVisualState();
    }

    private void UpdateVisualState()
    {
        if (buttonText == null)
            return;

        if (aartiController != null &&
            aartiController.IsPlaying)
        {
            buttonText.text = stopLabel;
        }
        else
        {
            buttonText.text = startLabel;
        }
    }
}