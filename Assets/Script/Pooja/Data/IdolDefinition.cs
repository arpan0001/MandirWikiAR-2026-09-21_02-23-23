using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "IdolDefinition",
    menuName = "Pooja/Idol Definition"
)]
public class IdolDefinition : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string id;
    [SerializeField] private string displayName;

    [Header("Available Rituals")]
    [SerializeField] private List<RitualDefinition> rituals;

    public string Id => id;

    public string DisplayName => displayName;

    public IReadOnlyList<RitualDefinition> Rituals => rituals;
}