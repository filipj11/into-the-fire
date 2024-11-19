using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveVisual : MonoBehaviour
{
    [SerializeField] private Stove stove;
    [SerializeField] private GameObject stoveOn;
    [SerializeField] private GameObject stoveParticles;

    private void Start()
    {
        stove.OnStateChange += Stove_OnStateChange;
    }

    private void Stove_OnStateChange(object sender, Stove.OnStateChangeEventArgs e)
    {
        bool showVis = e.state == Stove.State.Cooking || e.state == Stove.State.Cooked;

        stoveOn.SetActive(showVis);
        stoveParticles.SetActive(showVis);
    }
}
