using System.Collections.Generic;
using UnityEngine;

public class RitualRegistry : MonoBehaviour
{
    private readonly Dictionary<string, IRitualController> controllers =
        new Dictionary<string, IRitualController>();

    private void Awake()
    {
        RegisterRitualControllers();
    }

    private void RegisterRitualControllers()
    {
        MonoBehaviour[] behaviours =
            FindObjectsByType<MonoBehaviour>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

        foreach (MonoBehaviour behaviour in behaviours)
        {
            if (behaviour is IRitualController ritualController)
            {
                Register(ritualController);
            }
        }
    }

    private void Register(IRitualController controller)
    {
        if (controller == null)
            return;

        if (string.IsNullOrWhiteSpace(controller.RitualId))
        {
            Debug.LogWarning(
                "[RitualRegistry] Ritual controller has no ID."
            );

            return;
        }

        if (controllers.ContainsKey(controller.RitualId))
        {
            Debug.LogError(
                $"[RitualRegistry] Duplicate ritual ID: " +
                $"{controller.RitualId}"
            );

            return;
        }

        controllers.Add(
            controller.RitualId,
            controller
        );

        Debug.Log(
            $"[RitualRegistry] Registered ritual: " +
            $"{controller.RitualId}"
        );
    }

    public bool TryGetRitual(
        string ritualId,
        out IRitualController controller)
    {
        return controllers.TryGetValue(
            ritualId,
            out controller
        );
    }

    public void StopAllRituals()
    {
        foreach (IRitualController controller in controllers.Values)
        {
            controller.StopRitual();
        }
    }
}