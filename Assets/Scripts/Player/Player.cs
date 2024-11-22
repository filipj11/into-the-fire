using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public event EventHandler OnPickup;
    public event EventHandler<OnSelectedCounterChangeEventArgs> OnSelectedCounterChange;
    public class OnSelectedCounterChangeEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool isWalking;
    private Vector3 lastInteractionVector;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private void Start()
    {
        gameInput.OnInteraction += GameInput_OnInteraction;
        gameInput.OnInteractionAlt += GameInput_OnInteractionAlt;
    }

    private void GameInput_OnInteractionAlt(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsPlaying()) return;

        if (selectedCounter != null)
        {
            selectedCounter.InteractAlt(this);
        }
    }

    private void GameInput_OnInteraction(object sender, System.EventArgs e)
    {
        if (!GameManager.Instance.IsPlaying()) return;

        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void Update()
    {
        Vector2 inputVector = gameInput.GetNormalizedMovementVector();

        Vector3 moveVector = new Vector3(inputVector.x, 0f, inputVector.y);

        DoMovement(moveVector);
        DoInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void DoMovement(Vector3 moveVector)
    {
        float playerRadius = .55f;
        float playerHeight = 2f;
        float moveDistance = moveSpeed * Time.deltaTime;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveVector, moveDistance);

        if (!canMove)
        {
            // Check X component to allow movement beside walls while moving diagonally
            Vector3 moveVectorX = new Vector3(moveVector.x, 0f, 0f);

            canMove = moveVector.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveVectorX, moveDistance);

            if (canMove)
            {
                moveVector = moveVectorX.normalized;
            }
            else
            {
                // Check Z component to allow movement beside walls while moving diagonally
                Vector3 moveVectorZ = new Vector3(0f, 0f, moveVector.z);

                canMove = moveVector.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveVectorZ, moveDistance);

                if (canMove)
                {
                    moveVector = moveVectorZ.normalized;
                }
                else
                {
                    moveVector = Vector3.zero;
                }
            }
        } 
        
        transform.position += moveVector * moveSpeed * Time.deltaTime;

        isWalking = moveVector != Vector3.zero;

        if (isWalking)
        {
            float rotateSpeed = 30f;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveVector), Time.deltaTime * rotateSpeed);
        }
    }

    private void DoInteractions()
    {
        Vector2 inputVector = gameInput.GetNormalizedMovementVector();

        Vector3 moveVector = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveVector != Vector3.zero)
        {
            lastInteractionVector = moveVector;
        }

        float interactionDistance = 2f;

        if (Physics.Raycast(transform.position, lastInteractionVector, out RaycastHit raycastHit, interactionDistance, counterLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            } else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChange?.Invoke(this, new OnSelectedCounterChangeEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) 
    { 
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnPickup?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject() { return kitchenObject; }

    public void ClearKitchenObject() { this.kitchenObject = null; }

    public bool HasKitchenObject() { return this.kitchenObject != null; }
}
