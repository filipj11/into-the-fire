using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PantryCounterVisual : MonoBehaviour
{
    private const string OPEN_CLOSE = "OpenClose";

    [SerializeField] private PantryCounter pantryCounter;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        pantryCounter.OnPlayerInteract += PantryCounter_OnPlayerInteract;
    }

    private void PantryCounter_OnPlayerInteract(object sender, System.EventArgs e)
    {
        animator.SetTrigger(OPEN_CLOSE);
    }
}
