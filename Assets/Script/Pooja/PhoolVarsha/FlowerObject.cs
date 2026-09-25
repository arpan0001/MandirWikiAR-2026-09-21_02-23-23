using UnityEngine;

public class FlowerObject : MonoBehaviour
{
    private Vector3 velocity;
    private float rotationSpeed;

    private float fallSpeed;
    private float horizontalDrift;

    private bool isActive;

    public bool IsActive => isActive;

    public void Activate(
        Vector3 spawnPosition,
        float speed,
        float drift,
        float rotation)
    {
        transform.position = spawnPosition;

        transform.rotation =
            Random.rotation;

        fallSpeed = speed;
        horizontalDrift = drift;
        rotationSpeed = rotation;

        velocity = Vector3.zero;

        isActive = true;
        gameObject.SetActive(true);
    }

    public void Simulate()
    {
        if (!isActive)
            return;

        // Continuous downward movement
        transform.position +=
            Vector3.down *
            fallSpeed *
            Time.deltaTime;

        // Small horizontal movement
        transform.position +=
            Vector3.right *
            horizontalDrift *
            Time.deltaTime;

        // Natural flower rotation
        transform.Rotate(
            Vector3.up,
            rotationSpeed * Time.deltaTime,
            Space.Self
        );
    }

    public void Recycle()
    {
        isActive = false;
        gameObject.SetActive(false);
    }
}