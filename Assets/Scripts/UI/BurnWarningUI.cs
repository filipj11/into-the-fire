using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnWarningUI : MonoBehaviour
{
    [SerializeField] private Stove stove;

    private void Start()
    {
        stove.OnProgressChange += Stove_OnProgressChange;

        Hide();
    }

    private void Stove_OnProgressChange(object sender, IHasProgress.OnProgressChangeEventArgs e)
    {
        float showProgressAmount = .5f;
        bool showWarning = stove.IsCooked() && e.progressNormalized >= showProgressAmount;

        if (showWarning)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
