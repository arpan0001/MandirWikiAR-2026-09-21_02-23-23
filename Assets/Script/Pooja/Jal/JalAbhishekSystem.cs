using System.Collections;
using UnityEngine;

public class JalAbhishekSystem : MonoBehaviour
{
    [Header("Kalash")]
    [SerializeField]
    private GameObject kalashPrefab;

    [SerializeField]
    private Transform kalashAnchor;

    [Header("Water")]
    [SerializeField]
    private WaterStream waterStream;

    [SerializeField]
    private WaterSplashSystem splashSystem;

    [SerializeField]
    private Transform abhishekTarget;

    private GameObject spawnedKalash;
    private KalashController kalashController;

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
                PlayJalAbhishekSequence()
            );
    }

    private IEnumerator PlayJalAbhishekSequence()
    {
        // --------------------------------
        // 1. Spawn Kalash
        // --------------------------------

        SpawnKalash();

        if (spawnedKalash == null)
        {
            IsPlaying = false;
            yield break;
        }

        // --------------------------------
        // 2. Get Kalash Controller
        // --------------------------------

        kalashController =
            spawnedKalash.GetComponent<KalashController>();

        if (kalashController == null)
        {
            Debug.LogError(
                "[JalAbhishekSystem] " +
                "Kalash prefab requires KalashController."
            );

            Stop();
            yield break;
        }

        // --------------------------------
        // 3. Configure water stream
        // --------------------------------

        waterStream.Configure(
            kalashController.PourPoint,
            abhishekTarget
        );

        // --------------------------------
        // 4. Move + Tilt Kalash
        // --------------------------------

        yield return
            StartCoroutine(
                kalashController.PlaySequence(
                    kalashAnchor,
                    StartWater
                )
            );
    }

    private void SpawnKalash()
    {
        if (kalashPrefab == null)
        {
            Debug.LogError(
                "[JalAbhishekSystem] " +
                "Kalash prefab is not assigned."
            );

            return;
        }

        if (kalashAnchor == null)
        {
            Debug.LogError(
                "[JalAbhishekSystem] " +
                "Kalash anchor is not assigned."
            );

            return;
        }

        spawnedKalash =
            Instantiate(
                kalashPrefab,
                kalashAnchor.position,
                kalashAnchor.rotation
            );

        spawnedKalash.name =
            "Runtime_Kalash";
    }

    private void StartWater()
    {
        if (!IsPlaying)
            return;

        // Water stream
        if (waterStream != null)
        {
            waterStream.gameObject.SetActive(true);
        }

        // Splash
        if (splashSystem != null)
        {
            splashSystem.Play();
        }
    }

    public void Stop()
    {
        IsPlaying = false;

        // Stop sequence coroutine
        if (ritualCoroutine != null)
        {
            StopCoroutine(ritualCoroutine);
            ritualCoroutine = null;
        }

        // Stop splash
        if (splashSystem != null)
            splashSystem.Stop();

        // Stop water
        if (waterStream != null)
            waterStream.gameObject.SetActive(false);

        // Destroy spawned Kalash
        if (spawnedKalash != null)
        {
            Destroy(spawnedKalash);
            spawnedKalash = null;
        }

        kalashController = null;
    }

    private bool ValidateReferences()
    {
        if (kalashPrefab == null)
        {
            Debug.LogError(
                "[JalAbhishekSystem] " +
                "Kalash Prefab is missing."
            );

            return false;
        }

        if (kalashAnchor == null)
        {
            Debug.LogError(
                "[JalAbhishekSystem] " +
                "Kalash Anchor is missing."
            );

            return false;
        }

        if (waterStream == null)
        {
            Debug.LogError(
                "[JalAbhishekSystem] " +
                "Water Stream is missing."
            );

            return false;
        }

        if (abhishekTarget == null)
        {
            Debug.LogError(
                "[JalAbhishekSystem] " +
                "Abhishek Target is missing."
            );

            return false;
        }

        return true;
    }
}