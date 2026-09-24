using System.Collections.Generic;
using UnityEngine;

public class WaterSplashSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private WaterDropPool dropPool;

    [SerializeField]
    private Transform impactPoint;

    [Header("Spawn")]
    [SerializeField]
    private float spawnInterval = 0.12f;

    [SerializeField]
    private int dropsPerBurst = 2;

    [Header("Movement")]
    [SerializeField]
    private float minimumSpeed = 0.08f;

    [SerializeField]
    private float maximumSpeed = 0.18f;

    [SerializeField]
    private float gravity = 0.25f;

    [SerializeField]
    private float lifetime = 0.5f;

    private float timer;

    private bool isPlaying;

    public bool IsPlaying => isPlaying;

    private void Update()
    {
        if (!isPlaying)
            return;

        UpdateSpawning();
        UpdateDrops();
    }

    public void Play()
    {
        isPlaying = true;
        timer = 0f;
    }

    public void Stop()
    {
        isPlaying = false;
        timer = 0f;

        if (dropPool != null)
        {
            dropPool.DeactivateAll();
        }
    }

    private void UpdateSpawning()
    {
        timer += Time.deltaTime;

        if (timer < spawnInterval)
            return;

        timer = 0f;

        for (int i = 0;
             i < dropsPerBurst;
             i++)
        {
            SpawnDrop();
        }
    }

    private void SpawnDrop()
    {
        if (dropPool == null ||
            impactPoint == null)
        {
            return;
        }

        WaterDropObject drop =
            dropPool.GetDrop();

        if (drop == null)
            return;

        Vector3 direction =
            GetRandomSplashDirection();

        float speed =
            Random.Range(
                minimumSpeed,
                maximumSpeed
            );

        drop.Activate(
            impactPoint.position,
            direction * speed,
            gravity,
            lifetime
        );
    }

    private void UpdateDrops()
    {
        IReadOnlyList<WaterDropObject>
            drops =
                dropPool.Drops;

        for (int i = 0;
             i < drops.Count;
             i++)
        {
            WaterDropObject drop =
                drops[i];

            if (!drop.IsActive)
                continue;

            drop.Simulate();

            if (drop.HasFinished())
            {
                drop.Deactivate();
            }
        }
    }

    private Vector3 GetRandomSplashDirection()
    {
        Vector3 direction =
            new Vector3(
                Random.Range(-0.7f, 0.7f),
                Random.Range(0.2f, 0.8f),
                Random.Range(-0.4f, 0.4f)
            );

        return direction.normalized;
    }
}