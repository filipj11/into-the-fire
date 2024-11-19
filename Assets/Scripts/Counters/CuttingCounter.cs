using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnCutAny;

    new public static void ResetStatics()
    {
        OnCutAny = null;
    }

    public event EventHandler<IHasProgress.OnProgressChangeEventArgs> OnProgressChange;
    public event EventHandler OnCut;

    [SerializeField] private CuttingSO[] cuttingRecipeScriptableObjectArray;

    private int cuttingProgress;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // Counter does not have a KitchenObject on it

            if (player.HasKitchenObject())
            {
                // Player is holding a KitchenObject
                player.GetKitchenObject().SetKitchenObjectParent(this);
                cuttingProgress = 0;

                CuttingSO cuttingRecipeScriptableObject = GetCuttingRecipeScriptableObject(GetKitchenObject().GetKitchenObjectSO());

                OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs
                {
                    progressNormalized = (float)cuttingProgress / cuttingRecipeScriptableObject.progressMax
                });
            }
            else
            {
                // Player is not holding anything
            }
        }
        else
        {
            // Counter has a KitchenObject on it

            if (player.HasKitchenObject())
            {
                // Player is holding a KitchenObject

                if (player.GetKitchenObject().TryGetPlate(out Plate plate))
                {
                    if (plate.TryAdd(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
            else
            {
                // Player is not holding a KitchenObject

                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlt(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            cuttingProgress++;

            OnCut?.Invoke(this, EventArgs.Empty);
            OnCutAny?.Invoke(this, EventArgs.Empty);

            CuttingSO cuttingRecipeScriptableObject = GetCuttingRecipeScriptableObject(GetKitchenObject().GetKitchenObjectSO());

            OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeScriptableObject.progressMax
            });

            if (cuttingProgress >= cuttingRecipeScriptableObject.progressMax)
            {
                KitchenObjectSO output = GetOutput(GetKitchenObject().GetKitchenObjectSO());

                GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(output, this);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO input)
    {
        CuttingSO cuttingRecipeScriptableObject = GetCuttingRecipeScriptableObject(input);

        return cuttingRecipeScriptableObject != null;
    }

    private KitchenObjectSO GetOutput(KitchenObjectSO input)
    {
        CuttingSO cuttingRecipeScriptableObject = GetCuttingRecipeScriptableObject(input);

        if (cuttingRecipeScriptableObject != null)
        {
            return cuttingRecipeScriptableObject.output;
        }
        else
        {
            return null;
        }
    } 

    private CuttingSO GetCuttingRecipeScriptableObject(KitchenObjectSO input)
    {
        foreach (CuttingSO cuttingRecipe in cuttingRecipeScriptableObjectArray)
        {
            if (cuttingRecipe.input == input)
            {
                return cuttingRecipe;
            }
        }

        return null;
    }
}
