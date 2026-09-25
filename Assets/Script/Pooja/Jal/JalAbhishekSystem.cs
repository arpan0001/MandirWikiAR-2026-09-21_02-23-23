using System.Collections;
using UnityEngine;

public class JalAbhishekSystem : MonoBehaviour
{
    [Header("Kalash")]
    [SerializeField]
    private GameObject kalash;

    [SerializeField]
    private KalashController kalashController;

    [SerializeField]
    private Transform kalashTargetAnchor;

    [Header("Water")]
    [SerializeField]
    private WaterStream waterStream;

    [SerializeField]
    private WaterSplashSystem splashSystem;

    [SerializeField]
    private Transform abhishekTarget;

    [Header("Timing")]
    [SerializeField]
    private float waterStartDelay = 0.1f;

    private Coroutine ritualCoroutine;

    public bool IsPlaying { get; private set; }

    private void Awake()
    {
        Stop();
    }

    public void Play()
    {
        if (IsPlaying)
            return;

        if (!ValidateReferences())
            return;

        IsPlaying = true;

        ritualCoroutine =
            StartCoroutine(
                PlaySequence()
            );
    }

    private IEnumerator PlaySequence()
    {
        // --------------------------------
        // 1. ENABLE KALASH
        // --------------------------------

        kalash.SetActive(true);

        kalashController.ResetKalash();

        // --------------------------------
        // 2. Configure water
        // --------------------------------

        waterStream.Configure(
            kalashController.PourPoint,
            abhishekTarget
        );

        // --------------------------------
        // 3. Tilt Kalash
        // --------------------------------

        yield return StartCoroutine(
            kalashController.PlaySequence(
                kalashTargetAnchor,
                StartWater
            )
        );
    }

    private void StartWater()
    {
        if (!IsPlaying)
            return;

        if (waterStartDelay > 0f)
        {
            StartCoroutine(
                StartWaterDelayed()
            );

            return;
        }

        EnableWater();
    }

    private IEnumerator StartWaterDelayed()
    {
        yield return new WaitForSeconds(
            waterStartDelay
        );

        if (!IsPlaying)
            yield break;

        EnableWater();
    }

    private void EnableWater()
    {
        if (waterStream != null)
        {
            waterStream.gameObject.SetActive(true);
        }

        if (splashSystem != null)
        {
            splashSystem.Play();
        }
    }

    public void Stop()
    {
        IsPlaying = false;

        if (ritualCoroutine != null)
        {
            StopCoroutine(
                ritualCoroutine
            );

            ritualCoroutine = null;
        }

        if (splashSystem != null)
        {
            splashSystem.Stop();
        }

        if (waterStream != null)
        {
            waterStream.Stop();

            waterStream.gameObject.SetActive(false);
        }

        if (kalashController != null)
        {
            kalashController.ResetKalash();
        }

        if (kalash != null)
        {
            kalash.SetActive(false);
        }
    }

    private bool ValidateReferences()
    {
        if (kalash == null)
        {
            Debug.LogError(
                "[JalAbhishekSystem] " +
                "Kalash is missing."
            );

            return false;
        }

        if (kalashController == null)
        {
            Debug.LogError(
                "[JalAbhishekSystem] " +
                "KalashController is missing."
            );

            return false;
        }

        if (waterStream == null)
        {
            Debug.LogError(
                "[JalAbhishekSystem] " +
                "WaterStream is missing."
            );

            return false;
        }

        if (abhishekTarget == null)
        {
            Debug.LogError(
                "[JalAbhishekSystem] " +
                "AbhishekTarget is missing."
            );

            return false;
        }

        return true;
    }
}