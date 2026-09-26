using UnityEngine;
using DG.Tweening;

public class AartiDOTweenMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float radiusX = 0.25f;
    [SerializeField] private float radiusY = 0.15f;
    [SerializeField] private float duration = 2f;          // time for one full circle
    [SerializeField] private float introDuration = 0.25f;  // center -> top

    [Header("Smoothness")]
    [Range(8, 128)]
    [SerializeField] private int pathPoints = 32;          // more = rounder

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

        // Ease from center to the top first, then loop the circuit from the top.
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

    /// <summary>
    /// Generates points around the ellipse, starting at the top.
    /// The first point is NOT repeated at the end; the path is closed instead.
    /// </summary>
    private Vector3[] BuildPath()
    {
        int count = Mathf.Max(8, pathPoints);
        Vector3[] path = new Vector3[count];
        float dir = clockwise ? 1f : -1f;

        for (int i = 0; i < count; i++)
        {
            // t = 0 is the top; increasing t goes clockwise
            float t = (i / (float)count) * Mathf.PI * 2f;

            float x = Mathf.Sin(t) * radiusX * dir;
            float y = Mathf.Cos(t) * radiusY;

            path[i] = startPosition + new Vector3(x, y, 0f);
        }

        return path;
    }

    private void CreateAartiMovement(Vector3[] path)
    {
        if (!IsPlaying)
            return;

        aartiTween = transform
            .DOLocalPath(path, duration, PathType.CatmullRom)
            .SetOptions(true)                       // close the path so the loop is seamless
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