using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using HyperCasualPack;
using HyperCasualPack.Pickables;
using HyperCasualPack.Pools;
using TMPro;
using UnityEngine;

public class MarketShelf : PickableMarketShelfBase
{
    public PickableTypes MarketPickableType;
    public PickablePoolerSO neededPickablePool;
    public TMP_Text shelfStockUI;
    int totalShelvesInMarket = 0;
    void Start()
    {
         totalShelvesInMarket = ShelfSlots.Count;
    }

    private void OnEnable()
    {
        CustomerManager.Instance.CanStartSpawn();
    }

    public ShelfSlot GiveCustomerItem()
    {
        foreach (var item in ShelfSlots)
        {
            if (!item.isAvailable && item.Pickable)
            {
                currentCapacityOfShelf--;
                return item;
            }
        }

        return null;
    }

    void LateUpdate()
    {
        if(shelfStockUI != null)
            shelfStockUI.text = totalShelvesInMarket-CountOfAvailableShelf() + "/" + totalShelvesInMarket;
    }

    public int CountOfAvailableShelf()
    {
        int count = 0;
        for (int i = 0; i < ShelfSlots.Count; i++)
        {
            if (ShelfSlots[i].isAvailable)
                count++;
        }

        return count;
    }

    public bool CanPlaceInMarket()
    {
        foreach (var item in ShelfSlots)
        {
            if (item.isAvailable)
                return true;
        }

        return false;
    }
    protected override void OnJump(Pickable pickable)
    {
        var slotToSend = ReturnEmptySlot();
        slotToSend.isAvailable = false;
        slotToSend.Pickable = pickable;
        pickable.transform.DOMove(slotToSend.transform.position, 0.5f).OnComplete(() =>
        {
            
        });
    }

    public bool CheckForAI(PickablePoolerSO pickablePoolerSo)
    {
        if (neededPickablePool == pickablePoolerSo && CanPlaceInMarket())
        {
            return true;
        }

        return false;
    }

}
