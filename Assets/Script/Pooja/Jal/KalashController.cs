using System;
using System.Collections;
using UnityEngine;

public class KalashController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Transform kalashPivot;

    [SerializeField]
    private Transform pourPoint;

    [Header("Move")]
    [SerializeField]
    private float moveDuration = 0.6f;

    [Header("Tilt")]
    [SerializeField]
    private float tiltAngle = 55f;

    [SerializeField]
    private float tiltDuration = 0.8f;

    public Transform PourPoint => pourPoint;

    public bool IsTilted { get; private set; }

    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;
    private Quaternion initialPivotRotation;

    private void Awake()
    {
        if (kalashPivot == null)
        {
            Debug.LogError(
                "[KalashController] Kalash Pivot is not assigned."
            );

            return;
        }

        initialLocalPosition =
            transform.localPosition;

        initialLocalRotation =
            transform.localRotation;

        initialPivotRotation =
            kalashPivot.localRotation;
    }

    public void ResetKalash()
    {
        transform.localPosition =
            initialLocalPosition;

        transform.localRotation =
            initialLocalRotation;

        if (kalashPivot != null)
        {
            kalashPivot.localRotation =
                initialPivotRotation;
        }

        IsTilted = false;
    }

    public IEnumerator PlaySequence(
        Transform targetAnchor,
        Action onTiltComplete)
    {
        if (targetAnchor == null)
        {
            Debug.LogError(
                "[KalashController] " +
                "Target Anchor is missing."
            );

            yield break;
        }

        if (kalashPivot == null)
        {
            Debug.LogError(
                "[KalashController] " +
                "Kalash Pivot is missing."
            );

            yield break;
        }

        // --------------------------------
        // MOVE
        // --------------------------------

        Vector3 startPosition =
            transform.localPosition;

        Quaternion startRotation =
            transform.localRotation;

        Vector3 targetPosition =
            targetAnchor.localPosition;

        Quaternion targetRotation =
            targetAnchor.localRotation;

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;

            float t =
                Mathf.Clamp01(
                    elapsed / moveDuration
                );

            t = Mathf.SmoothStep(0f, 1f, t);

            transform.localPosition =
                Vector3.Lerp(
                    startPosition,
                    targetPosition,
                    t
                );

            transform.localRotation =
                Quaternion.Slerp(
                    startRotation,
                    targetRotation,
                    t
                );

            yield return null;
        }

        transform.localPosition =
            targetPosition;

        transform.localRotation =
            targetRotation;

        // --------------------------------
        // TILT
        // --------------------------------

        Quaternion startTilt =
            kalashPivot.localRotation;

        Quaternion endTilt =
            startTilt *
            Quaternion.Euler(
                0f,
                0f,
                -tiltAngle
            );

        elapsed = 0f;

        while (elapsed < tiltDuration)
        {
            elapsed += Time.deltaTime;

            float t =
                Mathf.Clamp01(
                    elapsed / tiltDuration
                );

            t = Mathf.SmoothStep(0f, 1f, t);

            kalashPivot.localRotation =
                Quaternion.Slerp(
                    startTilt,
                    endTilt,
                    t
                );

            yield return null;
        }

        kalashPivot.localRotation =
            endTilt;

        IsTilted = true;

        onTiltComplete?.Invoke();
    }
}