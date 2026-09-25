using UnityEngine;

public class WaterStream : MonoBehaviour
{
    [Header("Water Mesh")]
    [SerializeField]
    private Transform waterMesh;

    [Header("Stream Points")]
    [SerializeField]
    private Transform startPoint;

    [SerializeField]
    private Transform targetPoint;

    [Header("Mesh Settings")]
    [SerializeField]
    private float originalLength = 1f;

    [SerializeField]
    private float widthMultiplier = 1f;

    [SerializeField]
    private float depthMultiplier = 1f;

    [Header("Direction")]
    [SerializeField]
    private Vector3 meshFlowAxis =
        Vector3.up;

    private Vector3 initialScale;

    private bool isConfigured;

    private void Awake()
    {
        if (waterMesh == null)
        {
            Debug.LogError(
                "[WaterStream] Water Mesh is not assigned."
            );

            return;
        }

        initialScale =
            waterMesh.localScale;
    }

    public void Configure(
        Transform start,
        Transform target)
    {
        startPoint = start;
        targetPoint = target;

        if (startPoint == null)
        {
            Debug.LogError(
                "[WaterStream] Start Point is missing."
            );

            return;
        }

        if (targetPoint == null)
        {
            Debug.LogError(
                "[WaterStream] Target Point is missing."
            );

            return;
        }

        isConfigured = true;

        UpdateWater();
    }

    private void LateUpdate()
    {
        if (!isConfigured)
            return;

        UpdateWater();
    }

    private void UpdateWater()
    {
        if (waterMesh == null ||
            startPoint == null ||
            targetPoint == null)
        {
            return;
        }

        Vector3 start =
            startPoint.position;

        Vector3 target =
            targetPoint.position;

        Vector3 direction =
            target - start;

        float distance =
            direction.magnitude;

        if (distance < 0.001f)
            return;

        direction.Normalize();

        // ------------------------------
        // ROTATE MESH
        // ------------------------------

        Quaternion rotation =
            Quaternion.FromToRotation(
                meshFlowAxis.normalized,
                direction
            );

        waterMesh.rotation =
            rotation;

        // ------------------------------
        // POSITION
        // ------------------------------

        waterMesh.position =
            start;

        // ------------------------------
        // LENGTH
        // ------------------------------

        float lengthScale =
            distance /
            Mathf.Max(
                originalLength,
                0.001f
            );

        Vector3 scale =
            initialScale;

        scale.x *= widthMultiplier;

        scale.y *= lengthScale;

        scale.z *= depthMultiplier;

        waterMesh.localScale =
            scale;
    }

    public void Stop()
    {
        isConfigured = false;

        if (waterMesh != null)
        {
            waterMesh.localScale =
                initialScale;
        }
    }
}