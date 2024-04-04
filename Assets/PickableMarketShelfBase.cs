using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HyperCasualPack;
using HyperCasualPack.Pickables;
using HyperCasualPack.Pools;
using HyperCasualPack.ScriptableObjects;
using UnityEngine;

public abstract class PickableMarketShelfBase : MonoBehaviour
{
    [SerializeField] protected SaveableRuntimeIntVariable IncomeResource;
    [SerializeField, Tooltip("Selling jupm duration"), Range(0f, 10f)] float _jumpDuration;
    [SerializeField, Tooltip("Sell jump height"), Range(0f, 10f)] float _jumpHeight;
    [SerializeField, Tooltip("Sell count per second"), Range(0f, 20f)] float _sellRate;
    [SerializeField] private PickablePoolerSO pickableToSend;	
    Coroutine _cor;
    WaitForSeconds _waitForSeconds;
    public List<ShelfSlot> ShelfSlots;
    public int MaximumCapacityOfShelf;
    public int currentCapacityOfShelf;
    public Sprite marketShelfPng;
    void Awake()
    {
        _waitForSeconds = new WaitForSeconds(1f / _sellRate);
        if(ShelfSlots.Count == 0)
            ShelfSlots = GetComponentsInChildren<ShelfSlot>().ToList();
        MaximumCapacityOfShelf = ShelfSlots.Count;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out InventoryManager inventory))
        {
            _cor = StartCoroutine(CollectFromPlayer(inventory));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out InventoryManager _) && _cor != null)
        {
            StopCoroutine(_cor);
        }
    }

    IEnumerator CollectFromPlayer(InventoryManager inventoryManager)
    {
        if (currentCapacityOfShelf < MaximumCapacityOfShelf)
        {
            while (inventoryManager.TakePickable(pickableToSend,out Pickable pickable))
            {
                currentCapacityOfShelf++;
                PlaySequence(pickable);
                yield return _waitForSeconds;
            }    
        }
    }

    public IEnumerator GiveToAI(InventoryManager inventoryManager)
    {
        if (currentCapacityOfShelf < MaximumCapacityOfShelf)
        {
            while (inventoryManager.TakePickable(pickableToSend,out Pickable pickable))
            {
                currentCapacityOfShelf++;
                PlaySequence(pickable);
                yield return _waitForSeconds;
            }    
        }
    }
    
    public ShelfSlot ReturnEmptySlot()
    {
        var tempList = ShelfSlots;
        
        foreach (var item in tempList)
        {
            if (item.isAvailable)
                return item;
        }

        return null;
    }
    void PlaySequence(Pickable pickableItem)
    {
        OnJump(pickableItem);
    }

    protected abstract void OnJump(Pickable pickable);
}

