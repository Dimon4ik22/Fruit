using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HyperCasualPack;
using HyperCasualPack.Pickables;
using HyperCasualPack.Pools;
using UnityEngine;

public class PickableTreeSpawner : PickableCollectorBase
{
    [SerializeField] PickablePoolerSO _poolerSO;
    [SerializeField] Transform _stackingPoint;
    [SerializeField] public List<SeedSlot> seedSpawnerPositions;
    [SerializeField, Range(0f, 30f)] float _oreExtractionSec;
    [SerializeField] public PickableTreeCollector _pickableTreeCollector;
    float _currentTimer;
    int _workSpeedMultiplier;
    public bool isCocaoSeed;
    void Awake()
    {
        spawnedPickables = new Stack<Pickable>();
    }
    void Update()
    {
        var hasSlot = HasEmptySeedSlot();
        if (hasSlot != null)
        {
            _currentTimer += Time.deltaTime;
            if (_currentTimer >= _oreExtractionSec / (1 + _workSpeedMultiplier))
            {
                Spawn(hasSlot);
                _currentTimer = 0f;
            }
        }
    }
    protected override IEnumerator CollectFromPlayer(InventoryManager inventoryManager)
    {
        yield return null;
        
    }

    public override PickablePoolerSO GetPool()
    {
        return _poolerSO;
    }

    public bool HasUncollectedSeeds()
    {
        foreach (var item in seedSpawnerPositions)
        {
            if (item.isAvailable == false)
                return true;
        }

        return false;
    }
        
    void Spawn(SeedSlot slot)
    {
        Pickable pickable = _poolerSO.TakeFromPool();
        var getLocalScale = pickable.transform.localScale;
        pickable.transform.localScale = Vector3.zero;
        pickable.transform.SetPositionAndRotation(slot.slotPosition, Quaternion.identity);
        pickable.transform.DOScale(getLocalScale, 1f).OnComplete(() =>
        {
            slot.isAvailable = false;
            pickable.SeedSlot = slot;
            spawnedPickables.Push(pickable);
        });
    }

    public SeedSlot HasEmptySeedSlot()
    {
        foreach (var item in seedSpawnerPositions)
        {
            if (item.isAvailable)
                return item;
        }
        return null;
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (other.TryGetComponent(out ArcadeIdleMover arcadeIdleMover))
        {
            _pickableTreeCollector.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out ArcadeIdleMover arcadeIdleMover))
        {
            _pickableTreeCollector.gameObject.SetActive(true);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.TryGetComponent(out ArcadeIdleMover arcadeIdleMover))
        {
            _pickableTreeCollector.gameObject.SetActive(true);
        }
    }
}

[Serializable]
public class SeedSlot
{
    public bool isAvailable { get; set; }
    public Vector3 slotPosition { get; set; }

    public SeedSlot(bool _isAvailable,Vector3 _slotPosition)
    {
        this.isAvailable = _isAvailable;
        this.slotPosition = _slotPosition;

    }
}
