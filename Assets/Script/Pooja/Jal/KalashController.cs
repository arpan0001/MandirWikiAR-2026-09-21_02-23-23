using System.Collections;
using UnityEngine;

public class KalashController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Transform kalashPivot;

    [SerializeField]
    private Transform pourPoint;

    [Header("Position Animation")]
    [SerializeField]
    private float moveDuration = 0.6f;

    [Header("Tilt Animation")]
    [SerializeField]
    private float tiltAngle = 55f;

    [SerializeField]
    private float tiltDuration = 0.8f;

    [Header("Water Delay")]
    [SerializeField]
    private float waterStartDelay = 0.1f;

    private Quaternion initialRotation;
    private Quaternion tiltedRotation;

    public Transform PourPoint => pourPoint;

    public bool IsTilted { get; private set; }

    private void Awake()
    {
        if (kalashPivot == null)
            kalashPivot = transform;

        initialRotation = kalashPivot.localRotation;

        tiltedRotation =
            initialRotation *
            Quaternion.Euler(0f, 0f, -tiltAngle);
    }

    public IEnumerator PlaySequence(
        Transform spawnAnchor,
        System.Action onWaterStart)
    {
        if (spawnAnchor == null)
        {
            Debug.LogError(
                "[KalashController] Spawn anchor is not assigned."
            );

            yield break;
        }

        if (kalashPivot == null)
        {
            Debug.LogError(
                "[KalashController] Kalash pivot is not assigned."
            );

            yield break;
        }

        // --------------------------------
        // STEP 1: Move Kalash into position
        // --------------------------------

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        Vector3 targetPosition =
            spawnAnchor.position;

        Quaternion targetRotation =
            spawnAnchor.rotation;

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;

            float t =
                Mathf.Clamp01(
                    elapsed / moveDuration
                );

            t = Mathf.SmoothStep(0f, 1f, t);

            transform.position =
                Vector3.Lerp(
                    startPosition,
                    targetPosition,
                    t
                );

            transform.rotation =
                Quaternion.Slerp(
                    startRotation,
                    targetRotation,
                    t
                );

            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = targetRotation;

        // --------------------------------
        // STEP 2: Tilt Kalash
        // --------------------------------

        elapsed = 0f;

        Quaternion rotationBeforeTilt =
            kalashPivot.localRotation;

        Quaternion rotationAfterTilt =
            rotationBeforeTilt *
            Quaternion.Euler(
                0f,
                0f,
                -tiltAngle
            );

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
                    rotationBeforeTilt,
                    rotationAfterTilt,
                    t
                );

            yield return null;
        }

        kalashPivot.localRotation =
            rotationAfterTilt;

        IsTilted = true;

        // --------------------------------
        // STEP 3: Start water
        // --------------------------------

        if (waterStartDelay > 0f)
            yield return new WaitForSeconds(
                waterStartDelay
            );

        onWaterStart?.Invoke();
    }

    public void ResetKalash()
    {
        if (kalashPivot != null)
            kalashPivot.localRotation = initialRotation;

        IsTilted = false;
    }
}