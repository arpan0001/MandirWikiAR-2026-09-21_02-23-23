using UnityEngine;

public class WaterStream : MonoBehaviour
{
    public enum PivotMode
    {
        Top,
        Center
    }

    [Header("Water Mesh")]
    [SerializeField]
    private Transform waterMesh;

    [SerializeField]
    private PivotMode pivotMode = PivotMode.Top;

    [Header("References")]
    [SerializeField]
    private Transform startPoint;

    [SerializeField]
    private Transform targetPoint;

    [Header("Mesh Scaling")]
    [SerializeField]
    private float originalMeshLength = 1f;

    [SerializeField]
    private float widthMultiplier = 1f;

    [SerializeField]
    private float depthMultiplier = 1f;

    [Header("Runtime")]
    [SerializeField]
    private bool updateEveryFrame = true;

    private Vector3 initialScale;
    private Quaternion initialRotation;

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

        initialScale = waterMesh.localScale;
        initialRotation = waterMesh.rotation;
    }

    private void LateUpdate()
    {
        if (!isConfigured)
            return;

        if (!updateEveryFrame)
            return;

        UpdateWaterStream();
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
                "[WaterStream] Start Point is null."
            );

            return;
        }

        if (targetPoint == null)
        {
            Debug.LogError(
                "[WaterStream] Target Point is null."
            );

            return;
        }

        isConfigured = true;

        UpdateWaterStream();
    }

    public void SetWaterMesh(Transform mesh)
    {
        waterMesh = mesh;

        if (waterMesh != null)
        {
            initialScale = waterMesh.localScale;
            initialRotation = waterMesh.rotation;
        }
    }

    private void UpdateWaterStream()
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

        if (distance <= 0.001f)
            return;

        direction.Normalize();

        // --------------------------------
        // ROTATION
        // --------------------------------

        Quaternion rotation =
            Quaternion.FromToRotation(
                initialRotation * Vector3.up,
                direction
            ) *
            initialRotation;

        waterMesh.rotation = rotation;

        // --------------------------------
        // SCALE
        // --------------------------------

        float lengthScale =
            distance / Mathf.Max(
                originalMeshLength,
                0.001f
            );

        Vector3 scale =
            initialScale;

        scale.x *= widthMultiplier;
        scale.y *= lengthScale;
        scale.z *= depthMultiplier;

        waterMesh.localScale = scale;

        // --------------------------------
        // POSITION
        // --------------------------------

        if (pivotMode == PivotMode.Top)
        {
            // Mesh pivot is at the beginning
            // of the water stream.

            waterMesh.position = start;
        }
        else
        {
            // Mesh pivot is at the center.

            waterMesh.position =
                Vector3.Lerp(
                    start,
                    target,
                    0.5f
                );
        }
    }

    public void Stop()
    {
        isConfigured = false;

        if (waterMesh != null)
        {
            waterMesh.localScale =
                initialScale;

            waterMesh.rotation =
                initialRotation;
        }
    }
}