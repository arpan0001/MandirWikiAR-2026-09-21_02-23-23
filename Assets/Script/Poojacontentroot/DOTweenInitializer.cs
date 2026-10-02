using UnityEngine;
using DG.Tweening;

public class DOTweenInitializer : MonoBehaviour
{
    private void Awake()
    {
        DOTween.Init(
            recycleAllByDefault: false,
            useSafeMode: true,
            LogBehaviour.ErrorsOnly
        );
    }
}