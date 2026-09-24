using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class WaterStream : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Transform startPoint;

    [SerializeField]
    private Transform targetPoint;

    [Header("Curve")]
    [SerializeField]
    private int pointCount = 8;

    [SerializeField]
    private float curveAmount = 0.015f;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount =
            pointCount;
    }

    private void LateUpdate()
    {
        if (startPoint == null ||
            targetPoint == null)
        {
            return;
        }

        UpdateStream();
    }

    public void Configure(
        Transform start,
        Transform target)
    {
        startPoint = start;
        targetPoint = target;
    }

    private void UpdateStream()
    {
        Vector3 start =
            startPoint.position;

        Vector3 end =
            targetPoint.position;

        Vector3 direction =
            (end - start).normalized;

        Vector3 side =
            Vector3.Cross(
                direction,
                Vector3.up
            );

        if (side.sqrMagnitude < 0.001f)
        {
            side = Vector3.right;
        }

        side.Normalize();

        for (int i = 0;
             i < pointCount;
             i++)
        {
            float t =
                i / (float)(pointCount - 1);

            Vector3 position =
                Vector3.Lerp(
                    start,
                    end,
                    t
                );

            float curve =
                Mathf.Sin(t * Mathf.PI) *
                curveAmount;

            position +=
                side * curve;

            lineRenderer.SetPosition(
                i,
                position
            );
        }
    }
}