using UnityEngine;

public class FlowerObject : MonoBehaviour
{
    private Vector3 velocity;
    private Vector3 rotationSpeed;

    private float gravity;

    private bool isActive;

    public bool IsActive => isActive;

    public void Activate(
        Vector3 position,
        Vector3 initialVelocity,
        Vector3 initialRotationSpeed,
        float gravityValue)
    {
        transform.position = position;
        transform.rotation = Random.rotation;

        velocity = initialVelocity;
        rotationSpeed = initialRotationSpeed;
        gravity = gravityValue;

        isActive = true;

        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        isActive = false;

        velocity = Vector3.zero;
        rotationSpeed = Vector3.zero;

        gameObject.SetActive(false);
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    private void Update()
    {
        if (!isActive)
            return;

        UpdateMovement();
        UpdateRotation();
    }

    private void UpdateMovement()
    {
        velocity +=
            Vector3.down *
            gravity *
            Time.deltaTime;

        transform.position +=
            velocity *
            Time.deltaTime;
    }

    private void UpdateRotation()
    {
        transform.Rotate(
            rotationSpeed *
            Time.deltaTime,
            Space.Self
        );
    }
}