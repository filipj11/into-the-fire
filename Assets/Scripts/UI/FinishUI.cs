using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinishUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI successesText;
    [SerializeField] private TextMeshProUGUI failuresText;

    private void Start()
    {
        GameManager.Instance.OnStateChange += GameManager_OnStateChange;

        Hide();
    }

    private void GameManager_OnStateChange(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsFinish())
        {
            Show();

            successesText.text = DeliveryManager.Instance.GetSuccesses().ToString();
            failuresText.text = DeliveryManager.Instance.GetFailures().ToString();
        }
        else
        {
            Hide();
        }
    }

    private void Update()
    {
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
