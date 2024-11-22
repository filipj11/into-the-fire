using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawn;
    public event EventHandler OnRecipeComplete;
    public event EventHandler OnDeliverySuccess;
    public event EventHandler OnDeliveryFail;

    [SerializeField] private RecipeListSO recipeList;

    private List<RecipeSO> waitingList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;

    private int successes;
    private int failures;

    private void Awake()
    {
        waitingList = new List<RecipeSO>();
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;

        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (waitingList.Count < waitingRecipesMax)
            {
                RecipeSO waitingRecipe = recipeList.recipes[UnityEngine.Random.Range(0, recipeList.recipes.Count)];
                waitingList.Add(waitingRecipe);

                OnRecipeSpawn?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(Plate plate)
    {
        for (int i = 0; i < waitingList.Count; i++)
        {
            RecipeSO recipe = waitingList[i];

            bool ingredientsMatch = true;

            if (recipe.ingredients.Count == plate.GetPlateContents().Count)
            {
                foreach (KitchenObjectSO ingredient in recipe.ingredients)
                {
                    bool ingredientFound = false;

                    foreach (KitchenObjectSO plateIngredient in plate.GetPlateContents())
                    {
                        if (ingredient == plateIngredient)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }

                    if (!ingredientFound)
                    {
                        ingredientsMatch = false;
                    }
                }

                if (ingredientsMatch)
                {
                    // Player delivered a correct recipe

                    successes++;

                    waitingList.RemoveAt(i);

                    OnRecipeComplete?.Invoke(this, EventArgs.Empty);
                    OnDeliverySuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }

        // Player did not deliver a correct recipe
        failures++;

        OnDeliveryFail?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingList()
    {
        return waitingList;
    }

    public int GetSuccesses()
    {
        return successes;
    }

    public int GetFailures()
    {
        return failures;
    }
}
