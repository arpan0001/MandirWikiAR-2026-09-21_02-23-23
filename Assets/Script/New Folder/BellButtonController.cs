using UnityEngine;
using UnityEngine.UI;

public class BellButtonController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Button button;

    [SerializeField]
    private BellController bellController;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (button == null)
        {
            Debug.LogError(
                "[BellButtonController] " +
                "Button component is missing."
            );

            return;
        }

        button.onClick.AddListener(
            OnButtonClicked
        );
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
        if (bellController == null)
        {
            Debug.LogError(
                "[BellButtonController] " +
                "BellController is not assigned."
            );

            return;
        }

        bellController.Ring();
    }
}