using System.Collections.Generic;
using UnityEngine;

public class RamChantingSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private RamChant ramPrefab;

    [SerializeField]
    private Transform spawnCenter;

    [SerializeField]
    private Transform ramPool;

    [Header("Pool")]
    [SerializeField]
    private int poolSize = 20;

    [Header("Spawn")]
    [SerializeField]
    private float spawnInterval = 0.18f;

    [SerializeField]
    private float spawnRadius = 0.05f;

    [Header("Movement")]
    [SerializeField]
    private float minSpeed = 0.15f;

    [SerializeField]
    private float maxSpeed = 0.30f;

    [SerializeField]
    private float lifetime = 2.5f;

    [Header("Scale")]
    [SerializeField]
    private float startScale = 0.15f;

    [SerializeField]
    private float endScale = 0.45f;

    [Header("Fade")]
    [SerializeField]
    private float fadeStartTime = 1.5f;

    private readonly List<RamChant> pool =
        new List<RamChant>();

    private float spawnTimer;

    private void Awake()
    {
        CreatePool();
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;

            SpawnRam();
        }
    }

    private void CreatePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            RamChant ram =
                Instantiate(
                    ramPrefab,
                    ramPool
                );

            ram.gameObject.SetActive(false);

            pool.Add(ram);
        }
    }

    private void SpawnRam()
    {
        RamChant ram = GetAvailableRam();

        if (ram == null)
            return;

        // Random position close to center
        Vector3 spawnPosition =
            Random.insideUnitSphere *
            spawnRadius;

        // We mainly want movement in the XY plane
        spawnPosition.z = 0f;

        // Random direction in 360 degrees
        float angle =
            Random.Range(0f, Mathf.PI * 2f);

        Vector3 direction =
            new Vector3(
                Mathf.Cos(angle),
                Mathf.Sin(angle),
                0f
            );

        float speed =
            Random.Range(
                minSpeed,
                maxSpeed
            );

        ram.Initialize(
            spawnPosition,
            direction,
            speed,
            lifetime,
            startScale,
            endScale,
            fadeStartTime,
            ReturnToPool
        );
    }

    private RamChant GetAvailableRam()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].gameObject.activeSelf)
                return pool[i];
        }

        return null;
    }

    private void ReturnToPool(RamChant ram)
    {
        ram.gameObject.SetActive(false);
    }
}