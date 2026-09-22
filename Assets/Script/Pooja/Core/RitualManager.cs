using System;
using System.Collections;
using UnityEngine;

public class RitualManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private RitualRegistry ritualRegistry;

    private IRitualController activeController;
    private RitualDefinition activeDefinition;

    private Coroutine durationCoroutine;

    public RitualDefinition ActiveRitual =>
        activeDefinition;

    public bool HasActiveRitual =>
        activeController != null &&
        activeController.IsPlaying;

    public event Action<RitualDefinition> RitualStarted;
    public event Action<RitualDefinition> RitualStopped;

    public bool StartRitual(
        RitualDefinition definition)
    {
        if (definition == null)
        {
            Debug.LogWarning(
                "[RitualManager] RitualDefinition is null."
            );

            return false;
        }

        if (ritualRegistry == null)
        {
            Debug.LogError(
                "[RitualManager] RitualRegistry is not assigned."
            );

            return false;
        }

        if (!ritualRegistry.TryGetRitual(
                definition.Id,
                out IRitualController controller))
        {
            Debug.LogWarning(
                $"[RitualManager] No controller found for " +
                $"ritual: {definition.Id}"
            );

            return false;
        }

        // If the same ritual is already active,
        // do not start it again.
        if (activeController == controller &&
            controller.IsPlaying)
        {
            return false;
        }

        // Stop whatever ritual is currently active.
        StopActiveRitual();

        activeController = controller;
        activeDefinition = definition;

        activeController.StartRitual();

        RitualStarted?.Invoke(
            activeDefinition
        );

        StartDurationTimer(
            activeDefinition
        );

        Debug.Log(
            $"[RitualManager] Started ritual: " +
            $"{activeDefinition.DisplayName}"
        );

        return true;
    }

    public bool StopRitual(
        RitualDefinition definition)
    {
        if (definition == null)
            return false;

        if (activeDefinition == null)
            return false;

        if (activeDefinition.Id != definition.Id)
            return false;

        StopActiveRitual();

        return true;
    }

    public void StopActiveRitual()
    {
        StopDurationTimer();

        if (activeController == null)
        {
            activeDefinition = null;
            return;
        }

        RitualDefinition stoppedDefinition =
            activeDefinition;

        activeController.StopRitual();

        activeController = null;
        activeDefinition = null;

        RitualStopped?.Invoke(
            stoppedDefinition
        );

        Debug.Log(
            $"[RitualManager] Stopped ritual: " +
            $"{stoppedDefinition.DisplayName}"
        );
    }

    public bool IsActive(
        RitualDefinition definition)
    {
        if (definition == null)
            return false;

        return activeDefinition != null &&
               activeDefinition.Id == definition.Id &&
               activeController != null &&
               activeController.IsPlaying;
    }

    private void StartDurationTimer(
        RitualDefinition definition)
    {
        StopDurationTimer();

        if (definition.DurationSeconds <= 0f)
            return;

        durationCoroutine =
            StartCoroutine(
                DurationRoutine(
                    definition.DurationSeconds
                )
            );
    }

    private IEnumerator DurationRoutine(
        float duration)
    {
        yield return new WaitForSeconds(duration);

        StopActiveRitual();
    }

    private void StopDurationTimer()
    {
        if (durationCoroutine == null)
            return;

        StopCoroutine(durationCoroutine);

        durationCoroutine = null;
    }

    private void OnDestroy()
    {
        StopDurationTimer();
    }
}