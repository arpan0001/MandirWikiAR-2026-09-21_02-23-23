using UnityEngine;

public class PhoolVarshaEffect : MonoBehaviour
{
    [Header("Particle System")]
    [SerializeField]
    private ParticleSystem flowerParticles;

    public bool IsPlaying =>
        flowerParticles != null &&
        flowerParticles.isPlaying;

    public void Play()
    {
        if (flowerParticles == null)
        {
            Debug.LogWarning(
                "[PhoolVarshaEffect] " +
                "ParticleSystem is not assigned."
            );

            return;
        }

        flowerParticles.Play();
    }

    public void Stop()
    {
        if (flowerParticles == null)
            return;

        flowerParticles.Stop(
            true,
            ParticleSystemStopBehavior.StopEmittingAndClear
        );
    }
}