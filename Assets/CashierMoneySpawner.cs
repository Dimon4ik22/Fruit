using System.Collections;
using System.Collections.Generic;
using HyperCasualPack;
using HyperCasualPack.Pickables;
using HyperCasualPack.Pools;
using Sirenix.OdinInspector;
using UnityEngine;

public class CashierMoneySpawner : PickableCollectorBase
{
    [SerializeField] PickablePoolerSO _poolerSO;
    [SerializeField] Transform _stackingPoint;
    [SerializeField] public BoxCollider _pickableSourceCollector;

    void Awake()
    {
        spawnedPickables = new Stack<Pickable>();
    }
    [Button]
    public void SpawnXAmount(int x)
    {
        
        for (int i = 0; i < x; i++)
        {
            Spawn();
        }
    }
    void Spawn()
    {
        Pickable pickable = _poolerSO.TakeFromPool();
        pickable.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            
        JumpOrganized(pickable, _stackingPoint, spawnedPickables.Count);
        spawnedPickables.Push(pickable);
    }
    

    protected override IEnumerator CollectFromPlayer(InventoryManager inventoryManager)
    {
        return null;
    }

    public override PickablePoolerSO GetPool()
    {
        return _poolerSO;
    }
    
}
