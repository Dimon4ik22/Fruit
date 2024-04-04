using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HyperCasualPack.Pools;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace HyperCasualPack.Pickables
{
    public class PickableCollectorStockpiler : PickableCollectorBase, IUpgradable
    {
        [SerializeField] public PickableCollectorStockpilerRuleset _ruleset;
        [SerializeField] Transform _modifiedStockpilePoint;
        [SerializeField] Transform _unmodifiedStockpilePoint;

        [SerializeField, Tooltip("In seconds"), Range(0f, 10f)]
        float _modifyingTimePerItem;

        [SerializeField] float _stockpileEveryXSec;
        [SerializeField] private Transform modifyMovePos;
        [SerializeField] private Transform modifiedMovePos;
        [SerializeField] float stopMachineAfterXSeconds;
        [SerializeField] Animator machineAnimator;
        [SerializeField] TMP_Text factoryCapacityTxt;
        Stack<Pickable> _unmodifiedItems;
        float _currentModifiedItemTimer;
        float _currentStockpilingTimer;
        int _workSpeedMultiplier;
        int currentStockpiledAmount;
       public bool IsUnmodifiedItemCapacityFull => _unmodifiedItems.Count >= RowColumnHeight.ColumnCount * RowColumnHeight.RowCount * RowColumnHeight.HeightCount;
        
        void Awake()
        {
            spawnedPickables = new Stack<Pickable>();
            _unmodifiedItems = new Stack<Pickable>();
            factoryCapacityTxt.text = 0 + "/" + RowColumnHeight.ColumnCount * RowColumnHeight.RowCount * RowColumnHeight.HeightCount;
        }

        void Update()
        {
            if (_unmodifiedItems.Count == 0 && machineAnimator.GetBool("canProduce"))
            {
                Run.After(stopMachineAfterXSeconds, () =>
                {
                    machineAnimator.SetBool("canProduce", false);
                });
            }
        }

        void OnValidate()
        {
            JumpDuration = Mathf.Clamp(JumpDuration, 0f, Mathf.Min(_modifyingTimePerItem * 0.9f, _stockpileEveryXSec * 0.9f));
        }

        public void Upgrade()
        {
            _workSpeedMultiplier++;
        }

        protected override IEnumerator CollectFromPlayer(InventoryManager inventoryManager)
        {
            while (true)
            {
                if (IsUnmodifiedItemCapacityFull)
                {
                    yield return null;
                    continue;
                }

                if (_currentStockpilingTimer >= _stockpileEveryXSec)
                {
                    bool isFound = inventoryManager.ContainsPickable(_ruleset.InputPoolerSO);
                    if (isFound)
                    {
                        Run.After(_modifyingTimePerItem, () =>
                        {
                            machineAnimator.SetBool("canProduce", true);
                        });
                        if (inventoryManager.TakePickable(_ruleset.InputPoolerSO, out Pickable pickableItem))
                        {
                            currentStockpiledAmount++;
                            UpdateUIText();
                            JumpOrganized(pickableItem, _unmodifiedStockpilePoint, _unmodifiedItems.Count);
                            _unmodifiedItems.Push(pickableItem);
                            _currentStockpilingTimer = 0f;
                        }
                    }
                }
                else
                {
                    _currentStockpilingTimer += Time.deltaTime;
                }

                yield return null;
            }
        }

        public IEnumerator CollectFromAI(InventoryManager inventoryManager)
        {
          
                if (IsUnmodifiedItemCapacityFull)
                {
                    yield return null;
                }
                bool isFound = inventoryManager.ContainsPickable(_ruleset.InputPoolerSO);
                if (isFound)
                {
                    Run.After(_modifyingTimePerItem, () =>
                    {
                        machineAnimator.SetBool("canProduce", true);
                    });
                    if (inventoryManager.TakePickable(_ruleset.InputPoolerSO, out Pickable pickableItem))
                    {
                        currentStockpiledAmount++;
                        UpdateUIText();
                        JumpOrganized(pickableItem, _unmodifiedStockpilePoint, _unmodifiedItems.Count);
                        _unmodifiedItems.Push(pickableItem);
                        _currentStockpilingTimer = 0f;
                    }
                }
            
        }

        public override PickablePoolerSO GetPool()
        {
            return _ruleset.OutputPoolerSO;
        }

        Pickable ApplyModifying(Pickable previousPickable)
        {
            // previousPickable.DisappearSlowlyToPool();
            previousPickable.MoveThenDissappearSlowlyToPool(modifyMovePos, 0.3f);
            Pickable p = _ruleset.OutputPoolerSO.TakeFromPool();
            p.transform.position = previousPickable.transform.position;
            return p;
        }

        [Button]
        public void ModifyItem()
        {
            if (_unmodifiedItems.Count > 0)
            {
                Pickable pickableItemItem = _unmodifiedItems.Pop();
                Pickable p = ApplyModifying(pickableItemItem);
                var localScale = p.transform.localScale;
                p.transform.localScale = Vector3.zero;
                p.transform.DOScale(localScale, 0.3f);
                JumpOrganizedPrePos(p, _modifiedStockpilePoint, spawnedPickables.Count, modifiedMovePos);
                spawnedPickables.Push(p);
                _currentModifiedItemTimer = 0f;
            }
        }

        public void ModifyInstantiate()
        {
            if (_unmodifiedItems.Count > 0)
            {
                currentStockpiledAmount--;
                UpdateUIText();
                Pickable pickableItemItem = _unmodifiedItems.Pop();
                Pickable p = ApplyModifying(pickableItemItem);
                spawnedPickables.Push(p);
                p.transform.position = _modifiedStockpilePoint.position;
       
            }
        }

        public void UpdateUIText()
        {
        factoryCapacityTxt.text = currentStockpiledAmount + "/" + RowColumnHeight.ColumnCount * RowColumnHeight.RowCount * RowColumnHeight.HeightCount;
        }
    }
}