using System.Collections.Generic;
using UnityEngine;

public class FlowerPool : MonoBehaviour
{
    [Header("Pool")]
    [SerializeField]
    private FlowerObject[] flowerPrefabs;

    [SerializeField]
    private int flowersPerPrefab = 10;

    private readonly List<FlowerObject> flowers =
        new List<FlowerObject>();

    public IReadOnlyList<FlowerObject> Flowers =>
        flowers;

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        if (flowerPrefabs == null ||
            flowerPrefabs.Length == 0)
        {
            Debug.LogError(
                "[FlowerPool] No flower prefabs assigned."
            );

            return;
        }

        for (int i = 0; i < flowerPrefabs.Length; i++)
        {
            if (flowerPrefabs[i] == null)
                continue;

            for (int j = 0; j < flowersPerPrefab; j++)
            {
                FlowerObject flower =
                    Instantiate(
                        flowerPrefabs[i],
                        transform
                    );

                flower.gameObject.SetActive(false);

                flowers.Add(flower);
            }
        }
    }

    public FlowerObject GetFlower()
    {
        for (int i = 0; i < flowers.Count; i++)
        {
            if (!flowers[i].IsActive)
                return flowers[i];
        }

        return null;
    }

    public void DeactivateAll()
    {
        for (int i = 0; i < flowers.Count; i++)
        {
            if (flowers[i].IsActive)
                flowers[i].Recycle();
        }
    }
}