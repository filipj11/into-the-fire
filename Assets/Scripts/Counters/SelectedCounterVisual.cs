using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;
    [SerializeField] private Player player;

    private void Start()
    {
        player.OnSelectedCounterChange += Player_OnSelectedCounterChange;
    }

    private void Player_OnSelectedCounterChange(object sender, Player.OnSelectedCounterChangeEventArgs e)
    {
        if (e.selectedCounter == baseCounter)
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
        foreach (GameObject gameObject in visualGameObjectArray)
        {
            gameObject.SetActive(true);
        }
    }

    private void Hide() 
    {
        foreach (GameObject gameObject in visualGameObjectArray)
        {
            gameObject.SetActive(false);
        }
    }
}
