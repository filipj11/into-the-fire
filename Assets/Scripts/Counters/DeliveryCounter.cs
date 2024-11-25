using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    [SerializeField] private DeliveryManager deliveryManager;
    public static DeliveryCounter Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            if (player.GetKitchenObject().TryGetPlate(out Plate plate))
            {
                // Delivery should only accept plates
                deliveryManager.DeliverRecipe(plate);
                player.GetKitchenObject().DestroySelf();
            }
        }
    }
}
