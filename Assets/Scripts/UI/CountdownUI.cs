using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownUI : MonoBehaviour
{
    private const string NUMBER_POPUP = "Popup";

    [SerializeField] private TextMeshProUGUI countdownText;

    private Animator animator;
    private int previousCountdownNumber;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        GameManager.Instance.OnStateChange += GameManager_OnStateChange;

        Hide();
    }

    private void GameManager_OnStateChange(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsCountdown())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Update()
    {
        int currentCountdownNumber = Mathf.CeilToInt(GameManager.Instance.GetCountdownTimer());
        countdownText.text = currentCountdownNumber.ToString();

        if (previousCountdownNumber != currentCountdownNumber)
        {
            previousCountdownNumber = currentCountdownNumber;
            animator.SetTrigger(NUMBER_POPUP);
            SFXManager.Instance.PlayCountdownSFX();
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
