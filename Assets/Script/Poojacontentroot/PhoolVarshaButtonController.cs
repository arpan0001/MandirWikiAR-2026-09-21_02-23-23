using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhoolVarshaButtonController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Button button;

    [SerializeField]
    private PhoolVarshaSystem phoolVarshaSystem;

    [SerializeField]
    private TMP_Text buttonText;

    [Header("Labels")]
    [SerializeField]
    private string startLabel = "Phool Varsha";

    [SerializeField]
    private string stopLabel = "Stop Phool Varsha";

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(
                OnButtonClicked
            );
        }
        else
        {
            Debug.LogError(
                "[PhoolVarshaButtonController] " +
                "Button component not found."
            );
        }
    }

    private void Start()
    {
        UpdateVisual();
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(
                OnButtonClicked
            );
        }
    }

    private void OnButtonClicked()
    {
        if (phoolVarshaSystem == null)
        {
            Debug.LogWarning(
                "[PhoolVarshaButtonController] " +
                "PhoolVarshaSystem is not assigned."
            );

            return;
        }

        if (phoolVarshaSystem.IsPlaying)
        {
            phoolVarshaSystem.Stop();

            Debug.Log(
                "[PhoolVarshaButtonController] " +
                "Phool Varsha STOPPED."
            );
        }
        else
        {
            phoolVarshaSystem.Play();

            Debug.Log(
                "[PhoolVarshaButtonController] " +
                "Phool Varsha STARTED."
            );
        }

        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (buttonText == null)
            return;

        if (phoolVarshaSystem != null &&
            phoolVarshaSystem.IsPlaying)
        {
            buttonText.text = stopLabel;
        }
        else
        {
            buttonText.text = startLabel;
        }
    }
}