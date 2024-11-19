using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangeEventArgs> OnProgressChange;
    public event EventHandler<OnStateChangeEventArgs> OnStateChange;
    public class OnStateChangeEventArgs : EventArgs
    {
        public State state;
    }

    public enum State
    {
        Idle,
        Cooking,
        Cooked,
        Burned
    }


    [SerializeField] private CookingSO[] cookingRecipeScriptableObjectArray;
    [SerializeField] private BurningSO[] burningRecipeScriptableObjectArray;

    private State state;
    private float cookingTimer;
    private float burningTimer;
    private CookingSO cookingRecipeScriptableObject;
    private BurningSO burningRecipeScriptableObject;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Cooking:
                    cookingTimer += Time.deltaTime;

                    OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs
                    {
                        progressNormalized = cookingTimer / cookingRecipeScriptableObject.cookingTimerMax
                    });

                    if (cookingTimer > cookingRecipeScriptableObject.cookingTimerMax)
                    {
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(cookingRecipeScriptableObject.output, this);

                        burningRecipeScriptableObject = GetBurningRecipeScriptableObject(GetKitchenObject().GetKitchenObjectSO());
                        state = State.Cooked;
                        burningTimer = 0f;

                        OnStateChange?.Invoke(this, new OnStateChangeEventArgs
                        {
                            state = state
                        });
                    }
                    break;
                case State.Cooked:
                    burningTimer += Time.deltaTime;

                    OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs
                    {
                        progressNormalized = burningTimer / burningRecipeScriptableObject.burningTimerMax
                    });

                    if (burningTimer > burningRecipeScriptableObject.burningTimerMax)
                    {
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(burningRecipeScriptableObject.output, this);

                        state = State.Burned;

                        OnStateChange?.Invoke(this, new OnStateChangeEventArgs
                        {
                            state = state
                        });

                        OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                    break;
                case State.Burned:
                    break;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // Stove does not have a KitchenObject on it

            if (player.HasKitchenObject())
            {
                // Player is holding something 

                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    cookingRecipeScriptableObject = GetCookingRecipeScriptableObject(GetKitchenObject().GetKitchenObjectSO());

                    state = State.Cooking;
                    cookingTimer = 0f;

                    OnStateChange?.Invoke(this, new OnStateChangeEventArgs
                    {
                        state = state
                    });
                }
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

                        state = State.Idle;

                        OnStateChange?.Invoke(this, new OnStateChangeEventArgs
                        {
                            state = state
                        });

                        OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                }
            }
            else
            {
                // Player is not holding a KitchenObject

                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;

                OnStateChange?.Invoke(this, new OnStateChangeEventArgs
                {
                    state = state
                });

                OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO input)
    {
        CookingSO cookingRecipeScriptableObject = GetCookingRecipeScriptableObject(input);

        return cookingRecipeScriptableObject != null;
    }

    private KitchenObjectSO GetOutput(KitchenObjectSO input)
    {
        CookingSO cookingRecipeScriptableObject = GetCookingRecipeScriptableObject(input);

        if (cookingRecipeScriptableObject != null)
        {
            return cookingRecipeScriptableObject.output;
        }
        else
        {
            return null;
        }
    }

    private CookingSO GetCookingRecipeScriptableObject(KitchenObjectSO input)
    {
        foreach (CookingSO cookingRecipe in cookingRecipeScriptableObjectArray)
        {
            if (cookingRecipe.input == input)
            {
                return cookingRecipe;
            }
        }

        return null;
    }

    private BurningSO GetBurningRecipeScriptableObject(KitchenObjectSO input)
    {
        foreach (BurningSO burningRecipe in burningRecipeScriptableObjectArray)
        {
            if (burningRecipe.input == input)
            {
                return burningRecipe;
            }
        }

        return null;
    }

    public bool IsCooked()
    {
        return state == State.Cooked;
    }
}
