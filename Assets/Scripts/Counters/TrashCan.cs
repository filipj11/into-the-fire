using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : BaseCounter
{
    public static event EventHandler OnThrowAway;
    new public static void ResetStatics()
    {
        OnThrowAway = null;
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();

            OnThrowAway?.Invoke(this, EventArgs.Empty);
        }
    }
}
