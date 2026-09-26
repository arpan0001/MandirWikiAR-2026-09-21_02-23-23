using UnityEngine;
using TMPro;

public class RamChant : MonoBehaviour
{
    private TextMeshPro textMesh;

    private Vector3 direction;
    private float speed;
    private float lifetime;
    private float elapsedTime;

    private float startScale;
    private float endScale;

    private float fadeStartTime;

    private System.Action<RamChant> returnToPool;

    public void Initialize(
        Vector3 spawnPosition,
        Vector3 moveDirection,
        float moveSpeed,
        float lifeTime,
        float initialScale,
        float finalScale,
        float fadeStart,
        System.Action<RamChant> returnCallback)
    {
        transform.localPosition = spawnPosition;

        direction = moveDirection.normalized;
        speed = moveSpeed;
        lifetime = lifeTime;

        startScale = initialScale;
        endScale = finalScale;

        fadeStartTime = fadeStart;

        elapsedTime = 0f;

        returnToPool = returnCallback;

        transform.localScale =
            Vector3.one * startScale;

        if (textMesh == null)
            textMesh = GetComponent<TextMeshPro>();

        Color color = textMesh.color;
        color.a = 1f;
        textMesh.color = color;

        gameObject.SetActive(true);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        // Move outward
        transform.localPosition +=
            direction * speed * Time.deltaTime;

        // Grow slightly while moving
        float scaleT =
            Mathf.Clamp01(
                elapsedTime / lifetime
            );

        float currentScale =
            Mathf.Lerp(
                startScale,
                endScale,
                scaleT
            );

        transform.localScale =
            Vector3.one * currentScale;

        // Fade
        if (elapsedTime >= fadeStartTime)
        {
            float fadeT =
                Mathf.InverseLerp(
                    fadeStartTime,
                    lifetime,
                    elapsedTime
                );

            float alpha =
                Mathf.Lerp(
                    1f,
                    0f,
                    fadeT
                );

            Color color = textMesh.color;
            color.a = alpha;
            textMesh.color = color;
        }

        // Finished
        if (elapsedTime >= lifetime)
        {
            returnToPool?.Invoke(this);
        }
    }
}