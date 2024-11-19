using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimerUI : MonoBehaviour
{
    [SerializeField] private Image gameTimerImage;

    private void Update()
    {
        gameTimerImage.fillAmount = GameManager.Instance.GetNormalizedPlayingTimer();
    }
}
