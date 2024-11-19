using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform recipeContainer;
    [SerializeField] private Transform recipeTemplate;

    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSpawn += DeliveryManager_OnRecipeSpawn;
        DeliveryManager.Instance.OnRecipeComplete += DeliveryManager_OnRecipeComplete;
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

        foreach (RecipeSO recipe in DeliveryManager.Instance.GetWaitingList())
        {
            Transform recipeTransform = Instantiate(recipeTemplate, recipeContainer);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliveryManagerSingleRecipeUI>().SetRecipeIngredientsScriptableObject(recipe);
        }
    }
}
