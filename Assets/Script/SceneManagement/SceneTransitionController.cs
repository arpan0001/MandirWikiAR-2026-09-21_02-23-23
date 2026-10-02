using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionController : MonoBehaviour
{
    [SerializeField] private string storySceneName = "Story";

    public void LoadStoryScene()
    {
        if (string.IsNullOrEmpty(storySceneName))
        {
            Debug.LogError("[SceneTransitionController] Story scene name is empty.");
            return;
        }

        SceneManager.LoadScene(storySceneName);
    }
}