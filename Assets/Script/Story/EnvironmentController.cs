using System.Collections;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [Header("Environment")]
    [SerializeField] private GameObject grass;
    [SerializeField] private GameObject trees;
    [SerializeField] private GameObject mountains;
    [SerializeField] private GameObject rocks;
    [SerializeField] private GameObject clouds;

    [Header("Timing")]
    [SerializeField] private float delayBetweenElements = 1.5f;

    private Coroutine environmentRoutine;

    public void StartEnvironment()
    {
        if (environmentRoutine != null)
            StopCoroutine(environmentRoutine);

        environmentRoutine =
            StartCoroutine(EnvironmentSequence());
    }

    private IEnumerator EnvironmentSequence()
    {
        DisableAll();

        Debug.Log("[EnvironmentController] Grass.");

        if (grass != null)
            grass.SetActive(true);

        yield return new WaitForSeconds(
            delayBetweenElements
        );

        Debug.Log("[EnvironmentController] Trees.");

        if (trees != null)
            trees.SetActive(true);

        yield return new WaitForSeconds(
            delayBetweenElements
        );

        Debug.Log("[EnvironmentController] Mountains.");

        if (mountains != null)
            mountains.SetActive(true);

        yield return new WaitForSeconds(
            delayBetweenElements
        );

        Debug.Log("[EnvironmentController] Rocks.");

        if (rocks != null)
            rocks.SetActive(true);

        yield return new WaitForSeconds(
            delayBetweenElements
        );

        Debug.Log("[EnvironmentController] Clouds.");

        if (clouds != null)
            clouds.SetActive(true);

        Debug.Log(
            "[EnvironmentController] Environment complete."
        );

        environmentRoutine = null;
    }

    public void DisableAll()
    {
        SetActive(grass, false);
        SetActive(trees, false);
        SetActive(mountains, false);
        SetActive(rocks, false);
        SetActive(clouds, false);
    }

    private void SetActive(
        GameObject target,
        bool state
    )
    {
        if (target != null)
            target.SetActive(state);
    }
}