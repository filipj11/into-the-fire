using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectScriptableObject;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // Counter does not have a KitchenObject on it

            if (player.HasKitchenObject())
            {
                // Player is holding a KitchenObject

                player.GetKitchenObject().SetKitchenObjectParent(this);
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
                // Player is carrying something 

                if (player.GetKitchenObject().TryGetPlate(out Plate plate))
                {
                    // Player is holding a plate

                    if (plate.TryAdd(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    // Player is carrying something other than a plate
                    if (GetKitchenObject().TryGetPlate(out plate))
                    {
                        // Counter has plate on it
                        if (plate.TryAdd(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
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
}
