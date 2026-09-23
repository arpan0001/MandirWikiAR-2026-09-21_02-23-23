using UnityEngine;

public class PhoolVarshaController :
    MonoBehaviour,
    IRitualController
{
    [Header("Ritual Identity")]
    [SerializeField]
    private string ritualId = "phool_varsha";

    [Header("Components")]
    [SerializeField]
    private PhoolVarshaSystem flowerSystem;

    [SerializeField]
    private PhoolVarshaAudio audio;

    public string RitualId => ritualId;

    public bool IsPlaying =>
        flowerSystem != null &&
        flowerSystem.IsPlaying;

    public void StartRitual()
    {
        if (flowerSystem != null)
        {
            flowerSystem.Play();
        }

        if (audio != null)
        {
            audio.Play();
        }
    }

    public void StopRitual()
    {
        if (flowerSystem != null)
        {
            flowerSystem.Stop();
        }

        if (audio != null)
        {
            audio.Stop();
        }
    }
}