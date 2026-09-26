using UnityEngine;
using UnityEngine.InputSystem;

public class AartiDOTweenTest : MonoBehaviour
{
    [SerializeField] private AartiController aartiController;

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (aartiController.IsPlaying)
            {
                aartiController.StopRitual();
            }
            else
            {
                aartiController.StartRitual();
            }
        }
    }
}