using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AartiAudio : MonoBehaviour
{
    private AudioSource audioSource;

    public bool IsPlaying => audioSource != null && audioSource.isPlaying;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        if (audioSource == null)
            return;

        if (audioSource.clip == null)
        {
            Debug.LogWarning(
                "[AartiAudio] No AudioClip assigned."
            );

            return;
        }

        audioSource.Play();
    }

    public void Stop()
    {
        if (audioSource == null)
            return;

        audioSource.Stop();
    }

    public void Pause()
    {
        if (audioSource == null)
            return;

        audioSource.Pause();
    }

    public void Resume()
    {
        if (audioSource == null)
            return;

        audioSource.UnPause();
    }
}