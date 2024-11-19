using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnObjectPlace;

    public static void ResetStatics()
    {
        OnObjectPlace = null;
    }

    [SerializeField] private Transform countertopPoint;

    private KitchenObject kitchenObject;

    public virtual void Interact(Player player)
    {
        Debug.LogError("BaseCounter.Interact();");
    }

    public virtual void InteractAlt(Player player)
    {
        Debug.LogError("BaseCounter.InteractAlt();");
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return countertopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) 
    { 
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnObjectPlace?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject() { return kitchenObject; }

    public void ClearKitchenObject() { this.kitchenObject = null; }

    public bool HasKitchenObject() { return this.kitchenObject != null; }
}
