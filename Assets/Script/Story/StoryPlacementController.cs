using System.Collections;
using UnityEngine;

public class StoryPlacementController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform storyRoot;
    [SerializeField] private StoryDirector storyDirector;
    [SerializeField] private Camera arCamera;

    [Header("Automatic Start")]
    [SerializeField] private float startDelay = 3f;

    [Header("Placement")]
    [SerializeField] private bool faceCamera = true;

    private bool isPlaced;
    private Coroutine startCoroutine;

    public bool IsPlaced => isPlaced;

    private void Awake()
    {
        if (arCamera == null)
            arCamera = Camera.main;

        if (storyRoot != null)
            storyRoot.gameObject.SetActive(false);
    }

    public void PlaceStory(Pose groundPose)
    {
        if (isPlaced)
            return;

        if (storyRoot == null)
        {
            Debug.LogError(
                "[StoryPlacementController] StoryRoot is missing."
            );

            return;
        }

        Vector3 position = groundPose.position;

        Quaternion rotation = groundPose.rotation;

        if (faceCamera && arCamera != null)
        {
            Vector3 forward = arCamera.transform.forward;

            forward.y = 0f;

            if (forward.sqrMagnitude > 0.001f)
            {
                rotation = Quaternion.LookRotation(
                    forward.normalized,
                    Vector3.up
                );
            }
        }

        storyRoot.SetPositionAndRotation(
            position,
            rotation
        );

        storyRoot.gameObject.SetActive(true);

        isPlaced = true;

        Debug.Log(
            "[StoryPlacementController] StoryRoot placed."
        );

        if (storyDirector != null)
        {
            storyDirector.SetWaitingToStart();
        }

        StartAutomaticStory();
    }

    private void StartAutomaticStory()
    {
        if (startCoroutine != null)
            StopCoroutine(startCoroutine);

        startCoroutine =
            StartCoroutine(AutomaticStartRoutine());
    }

    private IEnumerator AutomaticStartRoutine()
    {
        Debug.Log(
            "[StoryPlacementController] Story starts in "
            + startDelay
            + " seconds."
        );

        yield return new WaitForSeconds(startDelay);

        if (!isPlaced)
            yield break;

        if (storyDirector != null)
        {
            storyDirector.StartStory();
        }

        startCoroutine = null;
    }

    public void ResetPlacement()
    {
        if (startCoroutine != null)
        {
            StopCoroutine(startCoroutine);
            startCoroutine = null;
        }

        isPlaced = false;

        if (storyRoot != null)
            storyRoot.gameObject.SetActive(false);

        if (storyDirector != null)
        {
            storyDirector.SetWaitingForPlacement();
        }
    }
}