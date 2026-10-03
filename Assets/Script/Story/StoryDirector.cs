using UnityEngine;

public class StoryDirector : MonoBehaviour
{
    public enum StoryState
    {
        NotStarted,
        WaitingForPlacement,
        WaitingToStart,
        Playing,
        ReadyForInteraction,
        Completed
    }

    public StoryState CurrentState { get; private set; }
        = StoryState.NotStarted;

    [SerializeField]
    private EnvironmentController environmentController;

    private void Awake()
    {
        CurrentState = StoryState.WaitingForPlacement;
    }

    public void SetWaitingForPlacement()
    {
        CurrentState = StoryState.WaitingForPlacement;

        Debug.Log("[StoryDirector] Waiting for Ground Plane.");
    }

    public void SetWaitingToStart()
    {
        CurrentState = StoryState.WaitingToStart;

        Debug.Log("[StoryDirector] Story placed. Waiting to start.");
    }

    public void StartStory()
    {
        if (CurrentState == StoryState.Playing)
            return;

        CurrentState = StoryState.Playing;

        Debug.Log("[StoryDirector] STORY STARTED.");

        if (environmentController != null)
        {
            environmentController.StartEnvironment();
        }
    }

    public void EnableInteraction()
    {
        CurrentState = StoryState.ReadyForInteraction;

        Debug.Log("[StoryDirector] Interaction enabled.");
    }

    public void StoryComplete()
    {
        CurrentState = StoryState.Completed;

        Debug.Log("[StoryDirector] Story completed.");
    }
}