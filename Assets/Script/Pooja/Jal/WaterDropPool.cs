using System.Collections.Generic;
using UnityEngine;

public class WaterDropPool : MonoBehaviour
{
    [Header("Pool")]
    [SerializeField]
    private WaterDropObject waterDropPrefab;

    [SerializeField]
    private int poolSize = 15;

    private readonly List<WaterDropObject> drops =
        new List<WaterDropObject>();

    public IReadOnlyList<WaterDropObject> Drops =>
        drops;

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        if (waterDropPrefab == null)
        {
            Debug.LogError(
                "[WaterDropPool] " +
                "WaterDrop prefab is not assigned."
            );

            return;
        }

        for (int i = 0;
             i < poolSize;
             i++)
        {
            WaterDropObject drop =
                Instantiate(
                    waterDropPrefab,
                    transform
                );

            drop.Deactivate();

            drops.Add(drop);
        }
    }

    public WaterDropObject GetDrop()
    {
        for (int i = 0;
             i < drops.Count;
             i++)
        {
            if (!drops[i].IsActive)
            {
                return drops[i];
            }
        }

        return null;
    }

    public void DeactivateAll()
    {
        for (int i = 0;
             i < drops.Count;
             i++)
        {
            if (drops[i].IsActive)
            {
                drops[i].Deactivate();
            }
        }
    }
}