using UnityEngine;

public class AartiMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float horizontalRadius = 0.12f;
    [SerializeField] private float verticalRadius = 0.08f;
    [SerializeField] private float duration = 2.5f;

    [Header("Orientation")]
    [SerializeField] private bool maintainInitialRotation = true;

    private Vector3 startPosition;
    private Quaternion initialRotation;

    private float elapsedTime;
    private bool isPlaying;

    public bool IsPlaying => isPlaying;

    private void Awake()
    {
        startPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    private void Update()
    {
        if (!isPlaying)
            return;

        UpdateMovement();
    }

    private void UpdateMovement()
    {
        if (duration <= 0f)
            return;

        elapsedTime += Time.deltaTime;

        float normalizedTime =
            (elapsedTime / duration) % 1f;

        float angle =
            normalizedTime * Mathf.PI * 2f;

        float x =
            Mathf.Sin(angle) * horizontalRadius;

        float y =
            Mathf.Cos(angle) * verticalRadius;

        transform.localPosition =
            startPosition +
            new Vector3(x, y, 0f);

        if (maintainInitialRotation)
        {
            transform.localRotation =
                initialRotation;
        }
    }

    public void Play()
    {
        elapsedTime = 0f;
        isPlaying = true;

        SetTopPosition();
    }

    public void Stop()
    {
        isPlaying = false;

        ResetMovement();
    }

    public void ResetMovement()
    {
        elapsedTime = 0f;

        transform.localPosition =
            startPosition;

        if (maintainInitialRotation)
        {
            transform.localRotation =
                initialRotation;
        }
    }

    private void SetTopPosition()
    {
        transform.localPosition =
            startPosition +
            new Vector3(
                0f,
                verticalRadius,
                0f
            );

        if (maintainInitialRotation)
        {
            transform.localRotation =
                initialRotation;
        }
    }
}