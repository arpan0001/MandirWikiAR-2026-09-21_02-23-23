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
    private PhoolVarshaEffect effect;

    [SerializeField]
    private PhoolVarshaAudio audio;

    public string RitualId => ritualId;

    public bool IsPlaying =>
        effect != null &&
        effect.IsPlaying;

    public void StartRitual()
    {
        if (effect != null)
        {
            effect.Play();
        }

        if (audio != null)
        {
            audio.Play();
        }
    }

    public void StopRitual()
    {
        if (effect != null)
        {
            effect.Stop();
        }

        if (audio != null)
        {
            audio.Stop();
        }
    }
}