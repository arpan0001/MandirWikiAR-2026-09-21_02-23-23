using System.Collections;
using UnityEngine;

public class StoryAutoStart : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private StoryDirector storyDirector;

    [Header("Timing")]
    [SerializeField] private float startDelay = 3f;

    private bool storyStarted;
    private Coroutine startRoutine;

    public void OnStoryPlaced()
    {
        if (storyStarted)
            return;

        if (startRoutine != null)
        {
            StopCoroutine(startRoutine);
        }

        startRoutine = StartCoroutine(StartStoryAfterDelay());
    }

    private IEnumerator StartStoryAfterDelay()
    {
        Debug.Log(
            "[StoryAutoStart] Story placed. " +
            "Starting story in " +
            startDelay +
            " seconds."
        );

        yield return new WaitForSeconds(startDelay);

        if (storyStarted)
            yield break;

        storyStarted = true;

        if (storyDirector != null)
        {
            storyDirector.StartStory();
        }
        else
        {
            Debug.LogError(
                "[StoryAutoStart] StoryDirector is not assigned."
            );
        }

        startRoutine = null;
    }

    public void ResetStory()
    {
        if (startRoutine != null)
        {
            StopCoroutine(startRoutine);
            startRoutine = null;
        }

        storyStarted = false;
    }
}