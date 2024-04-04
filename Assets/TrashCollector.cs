using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasualPack;
using HyperCasualPack.Pickables;
using HyperCasualPack.Pools;
using UnityEngine;

public class TrashCollector : PickableCollectorBase
{
    public List<PickablePoolerSO> allPickablesInTheScene;
    Stack<Pickable> _unmodifiedItems;
    public Transform _unmodifiedStockpilePoint;

    void Awake()
    {
        _unmodifiedItems = new Stack<Pickable>();
    }

    protected override IEnumerator CollectFromPlayer(InventoryManager inventoryManager)
    {
        for (int i = 0; i < allPickablesInTheScene.Count; i++)
        {
            if (inventoryManager.ContainsPickable(allPickablesInTheScene[i]))
            {
                if (inventoryManager.TakePickable(allPickablesInTheScene[i], out Pickable pickableItem))
                {
                    JumpOrganized(pickableItem, _unmodifiedStockpilePoint, _unmodifiedItems.Count);
                    _unmodifiedItems.Push(pickableItem);
                }
            }
        }

        yield return null;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out InventoryManager inventory))
        {
            _cor = StartCoroutine(CollectFromPlayer(inventory));
        }
    }

    public override PickablePoolerSO GetPool()
    {
        throw new System.NotImplementedException();
    }
}
