
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewDeityDefinition",
    menuName = "Pooja/Deity Definition"
)]
public class DeityDefinition : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string deityId;
    [SerializeField] private string displayName;

    [Header("Deity Prefab")]
    [SerializeField] private GameObject deityPrefab;

    [Header("Reveal")]
    [Min(0.1f)]
    [SerializeField] private float revealDuration = 1.2f;

    [SerializeField] private GameObject glowVFXPrefab;

    [Header("Audio")]
    [SerializeField] private AudioClip ambientAudio;
    [SerializeField] private AudioClip aartiAudio;

    [Header("Optional Effects")]
    [SerializeField] private GameObject deitySpecificVFXPrefab;

    [Header("Supported Rituals")]
    [SerializeField] private bool enableAarti = true;
    [SerializeField] private bool enablePhoolVarsha = true;
    [SerializeField] private bool enableBell = true;
    [SerializeField] private bool enableShankh = false;

    public string DeityId => deityId;
    public string DisplayName => displayName;
    public GameObject DeityPrefab => deityPrefab;
    public float RevealDuration => revealDuration;
    public GameObject GlowVFXPrefab => glowVFXPrefab;
    public AudioClip AmbientAudio => ambientAudio;
    public AudioClip AartiAudio => aartiAudio;
    public GameObject DeitySpecificVFXPrefab => deitySpecificVFXPrefab;

    public bool EnableAarti => enableAarti;
    public bool EnablePhoolVarsha => enablePhoolVarsha;
    public bool EnableBell => enableBell;
    public bool EnableShankh => enableShankh;
}
