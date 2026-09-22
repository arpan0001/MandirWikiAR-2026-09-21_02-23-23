using System;
using UnityEngine;
using Vuforia;

public class IdolTargetController : MonoBehaviour
{
    [Header("Idol Configuration")]
    [SerializeField] private IdolDefinition idolDefinition;

    [Header("Anchor")]
    [SerializeField] private Transform anchorTransform;

    private ObserverBehaviour observerBehaviour;
    private bool isTracked;

    public event Action<IdolDefinition> IdolDetected;
    public event Action<IdolDefinition> IdolLost;

    public IdolDefinition IdolDefinition => idolDefinition;
    public bool IsTracked => isTracked;
    public Transform AnchorTransform => anchorTransform;

    private void Awake()
    {
        observerBehaviour = GetComponent<ObserverBehaviour>();

        if (observerBehaviour == null)
        {
            Debug.LogError(
                $"[{nameof(IdolTargetController)}] " +
                $"ObserverBehaviour not found on {gameObject.name}."
            );

            return;
        }

        observerBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
    }

    private void OnDestroy()
    {
        if (observerBehaviour != null)
        {
            observerBehaviour.OnTargetStatusChanged -= OnTargetStatusChanged;
        }
    }

    private void OnTargetStatusChanged(
        ObserverBehaviour behaviour,
        TargetStatus targetStatus)
    {
        bool wasTracked = isTracked;

        isTracked = IsTracking(targetStatus.Status);

        if (!wasTracked && isTracked)
        {
            OnTargetDetected();
        }
        else if (wasTracked && !isTracked)
        {
            OnTargetLost();
        }
    }

    private bool IsTracking(Status status)
    {
        return status == Status.TRACKED ||
               status == Status.EXTENDED_TRACKED ||
               status == Status.LIMITED;
    }

    private void OnTargetDetected()
    {
        Debug.Log(
            $"[IdolTargetController] " +
            $"Detected idol: {idolDefinition?.DisplayName}"
        );

        IdolDetected?.Invoke(idolDefinition);
    }

    private void OnTargetLost()
    {
        Debug.Log(
            $"[IdolTargetController] " +
            $"Lost idol: {idolDefinition?.DisplayName}"
        );

        IdolLost?.Invoke(idolDefinition);
    }
}