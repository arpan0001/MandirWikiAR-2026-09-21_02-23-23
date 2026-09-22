using UnityEngine;

[CreateAssetMenu(
    fileName = "RitualDefinition",
    menuName = "Pooja/Ritual Definition"
)]
public class RitualDefinition : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string id;
    [SerializeField] private string displayName;
    [SerializeField] private Sprite icon;

    [Header("Content")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private AudioClip audioClip;

    [Header("Behaviour")]
    [SerializeField] private bool spawnOnDetection;
    [SerializeField] private float durationSeconds = 30f;

    public string Id => id;

    public string DisplayName => displayName;

    public Sprite Icon => icon;

    public GameObject Prefab => prefab;

    public AudioClip AudioClip => audioClip;

    public bool SpawnOnDetection => spawnOnDetection;

    public float DurationSeconds => durationSeconds;
}