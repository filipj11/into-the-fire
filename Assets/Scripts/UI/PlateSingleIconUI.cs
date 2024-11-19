using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateSingleIconUI : MonoBehaviour
{
    [SerializeField] private Image image;

    public void SetKitchenObjectScriptableObject(KitchenObjectSO kitchenObjectScriptableObject)
    {
        image.sprite = kitchenObjectScriptableObject.sprite;
    }
}