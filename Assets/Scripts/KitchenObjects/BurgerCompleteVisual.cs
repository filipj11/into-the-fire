using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct GameObjectKitchenObjectSO
    {
        public GameObject gameObject;
        public KitchenObjectSO kitchenObjectScriptableObject;
    }

    [SerializeField] private Plate plate;
    [SerializeField] private List<GameObjectKitchenObjectSO> burgerIngredients;

    private void Start()
    {
        plate.OnItemAdd += Plate_OnItemAdd;

        foreach (GameObjectKitchenObjectSO burgerIngredient in burgerIngredients)
        {
            burgerIngredient.gameObject.SetActive(false);
        }
    }

    private void Plate_OnItemAdd(object sender, Plate.OnItemAddEventArgs e)
    {
        foreach (GameObjectKitchenObjectSO burgerIngredient in burgerIngredients)
        {
            if (burgerIngredient.kitchenObjectScriptableObject == e.kitchenObjectScriptableObject) {
                burgerIngredient.gameObject.SetActive(true);
            }
            
        }
    }
}
