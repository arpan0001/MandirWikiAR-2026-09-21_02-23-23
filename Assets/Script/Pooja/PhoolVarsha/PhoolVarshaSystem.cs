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
    private Transform receivingZone;

    [Header("Spawn")]
    [SerializeField]
    private float spawnInterval = 0.12f;

    [SerializeField]
    private int flowersPerBurst = 1;

    [Header("Spawn Area")]
    [SerializeField]
    private Vector3 spawnAreaSize =
        new Vector3(0.5f, 0.05f, 0.5f);

    [Header("Receiving Zone")]
    [SerializeField]
    private Vector3 receivingZoneSize =
        new Vector3(0.3f, 0.35f, 0.2f);

    [Header("Movement")]
    [SerializeField]
    private float minimumFallSpeed = 0.3f;

    [SerializeField]
    private float maximumFallSpeed = 0.7f;

    [SerializeField]
    private float gravity = 0.15f;

    [Header("Rotation")]
    [SerializeField]
    private float minimumRotationSpeed = 60f;

    [SerializeField]
    private float maximumRotationSpeed = 180f;

    private float spawnTimer;

    private bool isPlaying;

    public bool IsPlaying => isPlaying;

    private void Update()
    {
        if (!isPlaying)
            return;

        UpdateSpawner();
        UpdateReceivingZone();
    }

    public void Play()
    {
        if (flowerPool == null)
        {
            Debug.LogError(
                "[PhoolVarshaSystem] " +
                "FlowerPool is not assigned."
            );

            return;
        }

        if (spawnArea == null)
        {
            Debug.LogError(
                "[PhoolVarshaSystem] " +
                "SpawnArea is not assigned."
            );

            return;
        }

        if (receivingZone == null)
        {
            Debug.LogError(
                "[PhoolVarshaSystem] " +
                "ReceivingZone is not assigned."
            );

            return;
        }

        isPlaying = true;
        spawnTimer = 0f;
    }

    public void Stop()
    {
        isPlaying = false;
        spawnTimer = 0f;

        if (flowerPool != null)
        {
            flowerPool.DeactivateAll();
        }
    }

    private void UpdateSpawner()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer < spawnInterval)
            return;

        spawnTimer = 0f;

        for (int i = 0;
             i < flowersPerBurst;
             i++)
        {
            SpawnFlower();
        }
    }

    private void SpawnFlower()
    {
        FlowerObject flower =
            flowerPool.GetFlower();

        if (flower == null)
            return;

        Vector3 spawnPosition =
            GetRandomSpawnPosition();

        Vector3 targetPosition =
            GetRandomReceivingPosition();

        Vector3 direction =
            (targetPosition - spawnPosition)
            .normalized;

        float fallSpeed =
            Random.Range(
                minimumFallSpeed,
                maximumFallSpeed
            );

        Vector3 velocity =
            direction * fallSpeed;

        Vector3 rotationSpeed =
            GetRandomRotationSpeed();

        flower.Activate(
            spawnPosition,
            velocity,
            rotationSpeed,
            gravity
        );
    }

    private void UpdateReceivingZone()
    {
        if (flowerPool == null ||
            receivingZone == null)
        {
            return;
        }

        IReadOnlyList<FlowerObject> flowers =
            flowerPool.PooledFlowers;

        for (int i = 0;
             i < flowers.Count;
             i++)
        {
            FlowerObject flower = flowers[i];

            if (!flower.IsActive)
                continue;

            if (IsInsideReceivingZone(
                    flower.transform.position))
            {
                flower.Deactivate();
            }
        }
    }

    private bool IsInsideReceivingZone(
        Vector3 worldPosition)
    {
        Vector3 localPosition =
            receivingZone.InverseTransformPoint(
                worldPosition
            );

        Vector3 halfSize =
            receivingZoneSize * 0.5f;

        return Mathf.Abs(localPosition.x)
                   <= halfSize.x
               &&
               Mathf.Abs(localPosition.y)
                   <= halfSize.y
               &&
               Mathf.Abs(localPosition.z)
                   <= halfSize.z;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 localOffset =
            new Vector3(
                Random.Range(
                    -spawnAreaSize.x * 0.5f,
                    spawnAreaSize.x * 0.5f
                ),

                Random.Range(
                    -spawnAreaSize.y * 0.5f,
                    spawnAreaSize.y * 0.5f
                ),

                Random.Range(
                    -spawnAreaSize.z * 0.5f,
                    spawnAreaSize.z * 0.5f
                )
            );

        return spawnArea.TransformPoint(
            localOffset
        );
    }

    private Vector3 GetRandomReceivingPosition()
    {
        Vector3 localOffset =
            new Vector3(
                Random.Range(
                    -receivingZoneSize.x * 0.5f,
                    receivingZoneSize.x * 0.5f
                ),

                Random.Range(
                    -receivingZoneSize.y * 0.5f,
                    receivingZoneSize.y * 0.5f
                ),

                Random.Range(
                    -receivingZoneSize.z * 0.5f,
                    receivingZoneSize.z * 0.5f
                )
            );

        return receivingZone.TransformPoint(
            localOffset
        );
    }

    private Vector3 GetRandomRotationSpeed()
    {
        float speed =
            Random.Range(
                minimumRotationSpeed,
                maximumRotationSpeed
            );

        return new Vector3(
            Random.Range(-speed, speed),
            Random.Range(-speed, speed),
            Random.Range(-speed, speed)
        );
    }
}