using UnityEngine;
using DG.Tweening;

public class AartiDOTweenMovement : MonoBehaviour
{
    public enum MovementPlane
    {
        FaceCamera, // circle always faces the AR camera (recommended for AR)
        XY,         // local X/Y
        XZ,         // local X/Z (flat circle)
        YZ          // local Y/Z
    }

    [Header("Movement")]
    [SerializeField] private float radiusX = 0.25f;
    [SerializeField] private float radiusY = 0.15f;
    [SerializeField] private float duration = 2f;
    [SerializeField] private float introDuration = 0.25f;

    [Header("Plane")]
    [SerializeField] private MovementPlane plane = MovementPlane.FaceCamera;
    [SerializeField] private Camera targetCamera;   // leave empty to use Camera.main

    [Header("Smoothness")]
    [Range(8, 128)]
    [SerializeField] private int pathPoints = 32;

    [Header("Direction")]
    [SerializeField] private bool clockwise = true;

    [Header("Animation")]
    [SerializeField] private Ease ease = Ease.Linear;

    private Tween introTween;
    private Tween aartiTween;
    private Vector3 startPosition;

    public bool IsPlaying { get; private set; }

    private void Awake()
    {
        startPosition = transform.localPosition;
    }

    public void Play()
    {
        if (IsPlaying)
            return;

        IsPlaying = true;
        KillTweens();

        Vector3[] path = BuildPath();

        introTween = transform.DOLocalMove(path[0], introDuration)
            .SetEase(Ease.OutSine)
            .SetLink(gameObject)
            .OnComplete(() => CreateAartiMovement(path));
    }

    public void Stop()
    {
        if (!IsPlaying)
            return;

        IsPlaying = false;
        KillTweens();
        transform.localPosition = startPosition;
    }

    // Returns the two local-space directions used as "right" and "up" for the circle
    private void GetAxes(out Vector3 right, out Vector3 up)
    {
        switch (plane)
        {
            case MovementPlane.XZ:
                right = Vector3.right;
                up = Vector3.forward;
                return;

            case MovementPlane.YZ:
                right = Vector3.forward;
                up = Vector3.up;
                return;

            case MovementPlane.FaceCamera:
                Camera cam = targetCamera != null ? targetCamera : Camera.main;
                if (cam != null)
                {
                    Vector3 worldRight = cam.transform.right;
                    Vector3 worldUp = cam.transform.up;

                    Transform parent = transform.parent;
                    if (parent != null)
                    {
                        right = parent.InverseTransformDirection(worldRight).normalized;
                        up = parent.InverseTransformDirection(worldUp).normalized;
                    }
                    else
                    {
                        right = worldRight;
                        up = worldUp;
                    }
                    return;
                }
                break; // no camera found, fall back to XY
        }

        right = Vector3.right;
        up = Vector3.up;
    }

    private Vector3[] BuildPath()
    {
        GetAxes(out Vector3 right, out Vector3 up);

        int count = Mathf.Max(8, pathPoints);
        Vector3[] path = new Vector3[count];
        float dir = clockwise ? 1f : -1f;

        for (int i = 0; i < count; i++)
        {
            float t = (i / (float)count) * Mathf.PI * 2f;

            float x = Mathf.Sin(t) * radiusX * dir;
            float y = Mathf.Cos(t) * radiusY;

            path[i] = startPosition + right * x + up * y;
        }

        return path;
    }

    private void CreateAartiMovement(Vector3[] path)
    {
        if (!IsPlaying)
            return;

        aartiTween = transform
            .DOLocalPath(path, duration, PathType.CatmullRom)
            .SetOptions(true)
            .SetEase(ease)
            .SetLoops(-1, LoopType.Restart)
            .SetLink(gameObject);
    }

    private void KillTweens()
    {
        introTween?.Kill();
        introTween = null;

        aartiTween?.Kill();
        aartiTween = null;
    }

    private void OnDisable()
    {
        KillTweens();
        IsPlaying = false;
        transform.localPosition = startPosition;
    }

    private void OnDestroy()
    {
        KillTweens();
    }
}