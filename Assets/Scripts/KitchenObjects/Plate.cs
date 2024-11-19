using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : KitchenObject
{
    public event EventHandler<OnItemAddEventArgs> OnItemAdd;
    public class OnItemAddEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectScriptableObject;
    }

    [SerializeField] private List<KitchenObjectSO> validKitchenObjectScriptableObjects;

    private List<KitchenObjectSO> plateContents;

    private void Awake()
    {
        plateContents = new List<KitchenObjectSO>();
    }

    public bool TryAdd(KitchenObjectSO kitchenObjectScriptableObject)
    {
        if (!validKitchenObjectScriptableObjects.Contains(kitchenObjectScriptableObject))
        {
            return false;
        }

        if (plateContents.Contains(kitchenObjectScriptableObject))
        {
            return false;
        }
        else
        {
            plateContents.Add(kitchenObjectScriptableObject);

            OnItemAdd?.Invoke(this, new OnItemAddEventArgs
            {
                kitchenObjectScriptableObject = kitchenObjectScriptableObject
            });

            return true;
        }
    }

    public List<KitchenObjectSO> GetPlateContents()
    {
        return plateContents;
    }
}
