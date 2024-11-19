using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnBarFlashUI : MonoBehaviour
{
    private const string IS_PULSING = "IsPulsing";

    [SerializeField] private Stove stove;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        stove.OnProgressChange += Stove_OnProgressChange;

        animator.SetBool(IS_PULSING, false);
    }

    private void Stove_OnProgressChange(object sender, IHasProgress.OnProgressChangeEventArgs e)
    {
        float showProgressAmount = .5f;
        bool showPulsing = stove.IsCooked() && e.progressNormalized >= showProgressAmount;

        animator.SetBool(IS_PULSING, showPulsing);
    }
}
