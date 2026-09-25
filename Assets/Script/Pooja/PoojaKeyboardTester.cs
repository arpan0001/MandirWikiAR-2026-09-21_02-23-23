using UnityEngine;
using UnityEngine.InputSystem;

public class PoojaKeyboardTester : MonoBehaviour
{
    [Header("Ritual Controllers")]
    [SerializeField]
    private AartiController aartiController;

    [SerializeField]
    private PhoolVarshaSystem phoolVarshaSystem;

    [SerializeField]
    private JalAbhishekController jalAbhishekController;

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        // 1 = Aarti
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            ToggleAarti();
        }

        // 2 = Phool Varsha
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            TogglePhoolVarsha();
        }

        // 3 = Jal Abhishek
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            ToggleJalAbhishek();
        }
    }

    private void ToggleAarti()
    {
        if (aartiController == null)
        {
            Debug.LogWarning(
                "[PoojaKeyboardTester] AartiController is not assigned."
            );

            return;
        }

        if (aartiController.IsPlaying)
        {
            aartiController.StopRitual();

            Debug.Log(
                "[PoojaKeyboardTester] Aarti STOPPED."
            );
        }
        else
        {
            aartiController.StartRitual();

            Debug.Log(
                "[PoojaKeyboardTester] Aarti STARTED."
            );
        }
    }

    private void TogglePhoolVarsha()
    {
        if (phoolVarshaSystem == null)
        {
            Debug.LogWarning(
                "[PoojaKeyboardTester] " +
                "PhoolVarshaSystem is not assigned."
            );

            return;
        }

        if (phoolVarshaSystem.IsPlaying)
        {
            phoolVarshaSystem.Stop();

            Debug.Log(
                "[PoojaKeyboardTester] " +
                "Phool Varsha STOPPED."
            );
        }
        else
        {
            phoolVarshaSystem.Play();

            Debug.Log(
                "[PoojaKeyboardTester] " +
                "Phool Varsha STARTED."
            );
        }
    }

    private void ToggleJalAbhishek()
    {
        if (jalAbhishekController == null)
        {
            Debug.LogWarning(
                "[PoojaKeyboardTester] " +
                "JalAbhishekController is not assigned."
            );

            return;
        }

        if (jalAbhishekController.IsPlaying)
        {
            jalAbhishekController.StopRitual();

            Debug.Log(
                "[PoojaKeyboardTester] " +
                "Jal Abhishek STOPPED."
            );
        }
        else
        {
            jalAbhishekController.StartRitual();

            Debug.Log(
                "[PoojaKeyboardTester] " +
                "Jal Abhishek STARTED."
            );
        }
    }
}