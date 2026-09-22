using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PhoolVarshaAudio : MonoBehaviour
{
    private AudioSource audioSource;

    public bool IsPlaying =>
        audioSource != null &&
        audioSource.isPlaying;

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
                "[PhoolVarshaAudio] " +
                "No AudioClip assigned."
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
}