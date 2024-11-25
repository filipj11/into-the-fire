using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform recipeContainer;
    [SerializeField] private Transform recipeTemplate;
    [SerializeField] private DeliveryManager deliveryManager;

    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        deliveryManager.OnRecipeSpawn += DeliveryManager_OnRecipeSpawn;
        deliveryManager.OnRecipeComplete += DeliveryManager_OnRecipeComplete;
    }

    private void DeliveryManager_OnRecipeComplete(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeSpawn(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in recipeContainer)
        {
            if (child == recipeTemplate)
            {
                continue;
            }

            Destroy(child.gameObject);
        }

        foreach (RecipeSO recipe in deliveryManager.GetWaitingList())
        {
            Transform recipeTransform = Instantiate(recipeTemplate, recipeContainer);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliveryManagerSingleRecipeUI>().SetRecipeIngredientsScriptableObject(recipe);
        }
    }
}
