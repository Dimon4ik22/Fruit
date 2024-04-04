using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;
using System.Xml;
using System.Xml.Linq;
using Sirenix.OdinInspector;

public class ShopManager : MonoBehaviourSingleton<ShopManager>
{
    public ItemDatabase itemDB;
    
    private void Awake()
    {
        LoadItems();
    }
    [Button]
    public void SaveItems()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ItemDatabase));
        FileStream stream =
            new FileStream(Application.dataPath + "/Base/Scripts/Systems/ShopSystem/StreamingFiles/XML/item_data.xml",
                FileMode.Create);
        serializer.Serialize(stream, itemDB);
        stream.Close();
        LoadItems();
    }
    private void OnApplicationQuit()
    {
        SaveItems();
        LoadItems();
    }
    [Button]
    public void LoadItems()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ItemDatabase));
        FileStream stream =
            new FileStream(Application.dataPath + "/Base/Scripts/Systems/ShopSystem/StreamingFiles/XML/item_data.xml",
                FileMode.Open);
        itemDB = serializer.Deserialize(stream) as ItemDatabase;
        stream.Close();
    }
    [Button]
    public void AddItem(ItemEntry itemToAdd)
    {
        itemDB.list.Add(itemToAdd);
        SaveItems();
    }
    [Button]
    public void BuyItem(string itemName)
    {
        var index = ShopManager.Instance.ReturnItemIndex(itemName);
        ShopManager.Instance.itemDB.list[index].hasBought = true;
        ShopManager.Instance.SaveItems();
    }
    [Button]
    public void RemoveItem(string itemName)
    {
        var index = ReturnItemIndex(itemName);
        itemDB.list.RemoveAt(index);
        SaveItems();
    }
    [Button]
    public List<ItemEntry> FindItemsByName(string itemName)
    {
        var items = itemDB.list.Where(a => a.itemName == itemName).ToList();
        return items;
    }
    [Button]
    public int ReturnItemIndex(string itemName)
    {
        int index = itemDB.list.Select((item, i) => new { Item = item.itemName, Index = i })
            .First(x => x.Item == itemName).Index;
        Debug.Log("Aradığın item listenin => " + index+". indexinde");
        return index;
    }
    [Button]
    public void AddCostToItem(string itemName,int amountToAdd)
    {
        int index = itemDB.list.Select((item, i) => new { Item = item.itemName, Index = i })
            .First(x => x.Item == itemName).Index;
        itemDB.list[index].price += amountToAdd;
    }
    [Button]
    public void CreateItemFromPool(string itemName)
    {
        try
        {
            var go = PoolingSystem.Instance.InstantiateAPS(itemName);
        }
        catch 
        {
            Debug.Log("Bu isimde bir Item Pool'da yok!");
        }
    }
}
[System.Serializable]
public class ItemEntry
{
    public string itemName;
    public int price;
    public bool hasBought = false;
    public ItemEntry()
    {
    }
}
[System.Serializable]
public class ItemDatabase
{
    //Eğer ilerde shop item , street shop vs gibi kategori eklenecekse database'ye XmlArray kullanarak xml dosyası parçalara bölünebilir.
    [XmlArray("ShopItems")]
    public List<ItemEntry> list = new List<ItemEntry>();
}