using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public string itemName;
    [ShowInInspector,ReadOnly]
    private int price;
    
    [SerializeField]
    private Texture itemUI;
    
    private void OnEnable()
    {
        try
        {
            price = ShopManager.Instance.itemDB.list[ShopManager.Instance.ReturnItemIndex(itemName)].price;
        }
        catch
        {
            Debug.Log("Bu isme ait bir Item database de bulunamadı");
        }   
    }
    [Button]
    public void OnItemBought()
    {
        ShopManager.Instance.BuyItem(itemName);
    }
}
