using System.Collections.Generic;
using UnityEngine;

public class PhoolVarshaSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private FlowerPool flowerPool;

    [SerializeField]
    private Transform spawnArea;

    [SerializeField]
    private Transform recycleArea;

    [Header("Spawn")]
    [SerializeField]
    private float spawnInterval = 0.12f;

    [SerializeField]
    private int flowersPerSpawn = 2;

    [Header("Flower Movement")]
    [SerializeField]
    private float minimumFallSpeed = 0.08f;

    [SerializeField]
    private float maximumFallSpeed = 0.14f;

    [SerializeField]
    private float maximumHorizontalDrift = 0.015f;

    [SerializeField]
    private float minimumRotationSpeed = 30f;

    [SerializeField]
    private float maximumRotationSpeed = 120f;

    private float spawnTimer;

    private bool isPlaying;

    public bool IsPlaying =>
        isPlaying;

    private void Update()
    {
        if (!isPlaying)
            return;

        UpdateFlowerMovement();
        UpdateSpawning();
    }

    // ============================================
    // START
    // ============================================

    public void Play()
    {
        if (isPlaying)
            return;

        if (!ValidateReferences())
            return;

        isPlaying = true;

        spawnTimer = 0f;

        // Immediately populate the shower.
        SpawnInitialFlowers();
    }

    // ============================================
    // STOP
    // ============================================

    public void Stop()
    {
        isPlaying = false;

        spawnTimer = 0f;

        if (flowerPool != null)
            flowerPool.DeactivateAll();
    }

    // ============================================
    // SPAWN
    // ============================================

    private void UpdateSpawning()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer < spawnInterval)
            return;

        spawnTimer -= spawnInterval;

        for (int i = 0; i < flowersPerSpawn; i++)
        {
            SpawnFlower();
        }
    }

    private void SpawnInitialFlowers()
    {
        for (int i = 0; i < 15; i++)
        {
            SpawnFlower();
        }
    }

    private void SpawnFlower()
    {
        FlowerObject flower =
            flowerPool.GetFlower();

        if (flower == null)
        {
            // Pool is currently full.
            // Existing flowers continue falling.
            return;
        }

        Vector3 spawnPosition =
            GetRandomSpawnPosition();

        float fallSpeed =
            Random.Range(
                minimumFallSpeed,
                maximumFallSpeed
            );

        float horizontalDrift =
            Random.Range(
                -maximumHorizontalDrift,
                maximumHorizontalDrift
            );

        float rotationSpeed =
            Random.Range(
                minimumRotationSpeed,
                maximumRotationSpeed
            );

        flower.Activate(
            spawnPosition,
            fallSpeed,
            horizontalDrift,
            rotationSpeed
        );
    }

    // ============================================
    // MOVEMENT + RECYCLING
    // ============================================

    private void UpdateFlowerMovement()
    {
        IReadOnlyList<FlowerObject> flowers =
            flowerPool.Flowers;

        for (int i = 0; i < flowers.Count; i++)
        {
            FlowerObject flower =
                flowers[i];

            if (!flower.IsActive)
                continue;

            flower.Simulate();

            if (HasReachedRecycleArea(flower))
            {
                RecycleAndImmediatelyRespawn(flower);
            }
        }
    }

    private void RecycleAndImmediatelyRespawn(
        FlowerObject flower)
    {
        // Do NOT deactivate and wait.
        // Immediately place the same flower
        // back at the top.

        Vector3 spawnPosition =
            GetRandomSpawnPosition();

        float fallSpeed =
            Random.Range(
                minimumFallSpeed,
                maximumFallSpeed
            );

        float horizontalDrift =
            Random.Range(
                -maximumHorizontalDrift,
                maximumHorizontalDrift
            );

        float rotationSpeed =
            Random.Range(
                minimumRotationSpeed,
                maximumRotationSpeed
            );

        flower.Activate(
            spawnPosition,
            fallSpeed,
            horizontalDrift,
            rotationSpeed
        );
    }

    // ============================================
    // SPAWN POSITION
    // ============================================

    private Vector3 GetRandomSpawnPosition()
    {
        Bounds bounds =
            GetBounds(spawnArea);

        return new Vector3(
            Random.Range(
                bounds.min.x,
                bounds.max.x
            ),

            Random.Range(
                bounds.min.y,
                bounds.max.y
            ),

            Random.Range(
                bounds.min.z,
                bounds.max.z
            )
        );
    }

    // ============================================
    // RECYCLE CHECK
    // ============================================

    private bool HasReachedRecycleArea(
        FlowerObject flower)
    {
        if (recycleArea == null)
            return false;

        Bounds bounds =
            GetBounds(recycleArea);

        return
            flower.transform.position.y
            <= bounds.max.y;
    }

    // ============================================
    // BOUNDS
    // ============================================

    private Bounds GetBounds(
        Transform area)
    {
        BoxCollider box =
            area.GetComponent<BoxCollider>();

        if (box != null)
        {
            Bounds bounds =
                box.bounds;

            return bounds;
        }

        return new Bounds(
            area.position,
            Vector3.one * 0.1f
        );
    }

    // ============================================
    // VALIDATION
    // ============================================

    private bool ValidateReferences()
    {
        if (flowerPool == null)
        {
            Debug.LogError(
                "[PhoolVarshaSystem] " +
                "FlowerPool is not assigned."
            );

            return false;
        }

        if (spawnArea == null)
        {
            Debug.LogError(
                "[PhoolVarshaSystem] " +
                "SpawnArea is not assigned."
            );

            return false;
        }

        if (recycleArea == null)
        {
            Debug.LogError(
                "[PhoolVarshaSystem] " +
                "RecycleArea is not assigned."
            );

            return false;
        }

        return true;
    }
}