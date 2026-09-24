using UnityEngine;

public class WaterDropObject : MonoBehaviour
{
    private Vector3 velocity;
    private float gravity;
    private float remainingLifetime;

    private bool isActive;

    public bool IsActive => isActive;

    public void Activate(
        Vector3 position,
        Vector3 initialVelocity,
        float gravityValue,
        float lifetime)
    {
        transform.position = position;

        transform.rotation =
            Random.rotation;

        velocity =
            initialVelocity;

        gravity =
            gravityValue;

        remainingLifetime =
            lifetime;

        isActive = true;

        gameObject.SetActive(true);
    }

    public void Simulate()
    {
        if (!isActive)
            return;

        velocity +=
            Vector3.down *
            gravity *
            Time.deltaTime;

        transform.position +=
            velocity *
            Time.deltaTime;

        remainingLifetime -=
            Time.deltaTime;
    }

    public bool HasFinished()
    {
        return remainingLifetime <= 0f;
    }

    public void Deactivate()
    {
        isActive = false;

        velocity = Vector3.zero;

        gameObject.SetActive(false);
    }
}