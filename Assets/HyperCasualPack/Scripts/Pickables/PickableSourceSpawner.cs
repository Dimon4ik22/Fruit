using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HyperCasualPack.Pools;
using UnityEngine;

namespace HyperCasualPack.Pickables
{
    public enum PickableSourceSpawnerType{Unlimited,LimitedAmount}
    public class PickableSourceSpawner : PickableCollectorBase, IUpgradable
    {
        [SerializeField] PickablePoolerSO _poolerSO;
        [SerializeField] Transform _stackingPoint;
        [SerializeField, Range(0f, 30f)] float _oreExtractionSec;
        public PickableSourceSpawnerType PickableSourceSpawnerType;
        bool canContinueSpawn = true;
        float _currentTimer;
        int _workSpeedMultiplier;
        
        void Awake()
        {
            spawnedPickables = new Stack<Pickable>();
        }

        void Update()
        {
            switch (PickableSourceSpawnerType)
            {
                case PickableSourceSpawnerType.Unlimited:
                    if (spawnedPickables.Count < RowColumnHeight.HeightCount * RowColumnHeight.RowCount * RowColumnHeight.ColumnCount)
                    {
                        _currentTimer += Time.deltaTime;

                        if (_currentTimer >= _oreExtractionSec / (1 + _workSpeedMultiplier))
                        {
                            Spawn();
                            _currentTimer = 0f;
                        }
                    }
                    break;
                case PickableSourceSpawnerType.LimitedAmount:
                    if (canContinueSpawn)
                    {
                        if (spawnedPickables.Count < RowColumnHeight.HeightCount * RowColumnHeight.RowCount * RowColumnHeight.ColumnCount)
                        {
                            _currentTimer += Time.deltaTime;

                            if (_currentTimer >= _oreExtractionSec / (1 + _workSpeedMultiplier))
                            {
                                Spawn();
                                _currentTimer = 0f;
                            }
                        }
                        if (spawnedPickables.Count == RowColumnHeight.HeightCount * RowColumnHeight.RowCount * RowColumnHeight.ColumnCount)
                            canContinueSpawn = false;
                    }
                    break;
            }

        }

        public void Upgrade()
        {
            _workSpeedMultiplier++;
        }
        
        protected override IEnumerator CollectFromPlayer(InventoryManager inventoryManager)
        {
            yield return null;
        }

        public override PickablePoolerSO GetPool()
        {
            return _poolerSO;
        }
        
        void Spawn()
        {
            Pickable pickable = _poolerSO.TakeFromPool();
            pickable.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            
            JumpOrganized(pickable, _stackingPoint, spawnedPickables.Count);
            spawnedPickables.Push(pickable);
        }
    }
}