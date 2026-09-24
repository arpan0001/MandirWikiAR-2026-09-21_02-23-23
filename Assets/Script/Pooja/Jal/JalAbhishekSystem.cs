using UnityEngine;

public class JalAbhishekSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private WaterStream waterStream;

    [SerializeField]
    private WaterSplashSystem splashSystem;

    public bool IsPlaying { get; private set; }

    private void Awake()
    {
        Stop();
    }

    public void Play()
    {
        if (waterStream == null)
        {
            Debug.LogError(
                "[JalAbhishekSystem] " +
                "WaterStream is not assigned."
            );

            return;
        }

        if (splashSystem == null)
        {
            Debug.LogError(
                "[JalAbhishekSystem] " +
                "SplashSystem is not assigned."
            );

            return;
        }

        IsPlaying = true;

        waterStream.gameObject.SetActive(true);

        splashSystem.Play();
    }

    public void Stop()
    {
        IsPlaying = false;

        if (splashSystem != null)
        {
            splashSystem.Stop();
        }

        if (waterStream != null)
        {
            waterStream.gameObject.SetActive(false);
        }
    }
}