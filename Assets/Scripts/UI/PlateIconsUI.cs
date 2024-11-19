using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private Plate plate;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
       iconTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        plate.OnItemAdd += Plate_OnItemAdd;
    }

    private void Plate_OnItemAdd(object sender, Plate.OnItemAddEventArgs e)
    {
        UpdateIcons();
    }

    private void UpdateIcons()
    {
        foreach (Transform child in transform)
        {
            if (child != iconTemplate)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (KitchenObjectSO item in plate.GetPlateContents())
        {
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<PlateSingleIconUI>().SetKitchenObjectScriptableObject(item);
        }
    }
}
