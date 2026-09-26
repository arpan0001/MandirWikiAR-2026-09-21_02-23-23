using System.Collections;
using UnityEngine;

public class PoojaContentController : MonoBehaviour
{
    [Header("Content Root")]
    [SerializeField]
    private GameObject contentRoot;

    [Header("Ritual Roots")]
    [SerializeField]
    private GameObject aartiRoot;

    [SerializeField]
    private GameObject phoolVarshaRoot;

    [SerializeField]
    private GameObject bellRoot;

    [Header("UI")]
    [SerializeField]
    private PoojaaUIController poojaaUIController;

    [Header("Animators")]
    [SerializeField]
    private Animator aartiAnimator;

    [SerializeField]
    private Animator phoolVarshaAnimator;

    [SerializeField]
    private Animator bellAnimator;

    [Header("Reveal Settings")]
    [SerializeField]
    private float revealDelay = 0.15f;

    private static readonly int RevealHash =
        Animator.StringToHash("Reveal");

    private bool poojaStarted;

    public bool PoojaStarted => poojaStarted;

    private void Awake()
    {
        HideAll();
    }

    public void StartPooja()
    {
        if (poojaStarted)
            return;

        poojaStarted = true;

        if (contentRoot != null)
            contentRoot.SetActive(true);

        if (poojaaUIController != null)
        {
            poojaaUIController.ShowRitualBar();
        }

        StartCoroutine(
            RevealRituals()
        );

        Debug.Log(
            "[PoojaContentController] " +
            "Pooja started."
        );
    }

    private IEnumerator RevealRituals()
    {
        // -----------------------------
        // AARTI
        // -----------------------------

        if (aartiRoot != null)
            aartiRoot.SetActive(true);

        if (aartiAnimator != null)
        {
            aartiAnimator.ResetTrigger(RevealHash);
            aartiAnimator.SetTrigger(RevealHash);
        }

        // -----------------------------
        // PHOOL VARSHA
        // -----------------------------

        yield return new WaitForSeconds(
            revealDelay
        );

        if (phoolVarshaRoot != null)
            phoolVarshaRoot.SetActive(true);

        if (phoolVarshaAnimator != null)
        {
            phoolVarshaAnimator.ResetTrigger(RevealHash);
            phoolVarshaAnimator.SetTrigger(RevealHash);
        }

        // -----------------------------
        // BELL
        // -----------------------------

        yield return new WaitForSeconds(
            revealDelay
        );

        if (bellRoot != null)
            bellRoot.SetActive(true);

        if (bellAnimator != null)
        {
            bellAnimator.ResetTrigger(RevealHash);
            bellAnimator.SetTrigger(RevealHash);
        }

        Debug.Log(
            "[PoojaContentController] " +
            "All ritual visuals revealed."
        );
    }

    public void ResetPooja()
    {
        poojaStarted = false;

        if (aartiAnimator != null)
        {
            aartiAnimator.ResetTrigger(RevealHash);
            aartiAnimator.Play(
                "Idle",
                0,
                0f
            );
            aartiAnimator.Update(0f);
        }

        if (phoolVarshaAnimator != null)
        {
            phoolVarshaAnimator.ResetTrigger(RevealHash);
            phoolVarshaAnimator.Play(
                "Idle",
                0,
                0f
            );
            phoolVarshaAnimator.Update(0f);
        }

        if (bellAnimator != null)
        {
            bellAnimator.ResetTrigger(RevealHash);
            bellAnimator.Play(
                "Idle",
                0,
                0f
            );
            bellAnimator.Update(0f);
        }

        if (contentRoot != null)
            contentRoot.SetActive(false);
    }

    public void HideAll()
    {
        poojaStarted = false;

        if (contentRoot != null)
            contentRoot.SetActive(false);
    }
}