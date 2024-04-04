using System.Collections;
using System.Collections.Generic;
using HyperCasualPack.Pickables;
using UnityEngine;

public class Factory : MonoBehaviour
{
    [SerializeField] PickableCollectorStockpiler _pickableCollectorStockpiler;
    public MarketShelf MarketShelfToGo;
    public GameObject FactoryCollectTransform;

    public void ModifyItem()
    {
        _pickableCollectorStockpiler.ModifyInstantiate();
    }

    public PickableCollectorStockpiler ReturnStockPiler()
    {
        return this._pickableCollectorStockpiler;
    }
}
