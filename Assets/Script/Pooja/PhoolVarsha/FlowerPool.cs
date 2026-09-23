using System.Collections.Generic;
using UnityEngine;

public class FlowerPool : MonoBehaviour
{
    [Header("Flower Prefabs")]
    [SerializeField]
    private List<FlowerObject> flowerPrefabs;

    [Header("Pool")]
    [SerializeField]
    private int poolSize = 30;

    [Header("Parent")]
    [SerializeField]
    private Transform poolParent;

    private readonly List<FlowerObject> pooledFlowers =
        new List<FlowerObject>();

    public IReadOnlyList<FlowerObject> PooledFlowers =>
        pooledFlowers;

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        if (flowerPrefabs == null ||
            flowerPrefabs.Count == 0)
        {
            Debug.LogError(
                "[FlowerPool] No flower prefabs assigned."
            );

            return;
        }

        for (int i = 0; i < poolSize; i++)
        {
            FlowerObject prefab =
                flowerPrefabs[
                    Random.Range(
                        0,
                        flowerPrefabs.Count
                    )
                ];

            FlowerObject flower =
                Instantiate(
                    prefab,
                    poolParent
                );

            flower.Deactivate();

            pooledFlowers.Add(flower);
        }
    }

    public FlowerObject GetFlower()
    {
        foreach (FlowerObject flower
                 in pooledFlowers)
        {
            if (!flower.IsActive)
            {
                return flower;
            }
        }

        return null;
    }

    public void DeactivateAll()
    {
        foreach (FlowerObject flower
                 in pooledFlowers)
        {
            if (flower.IsActive)
            {
                flower.Deactivate();
            }
        }
    }
}