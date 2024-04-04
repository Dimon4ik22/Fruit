using HyperCasualPack.Pools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualPack.Pickables
{
    public class PickableCollectorMultipleCondition : PickableCollectorBase
    {
        public event Action<PickablePoolerSO, int> Collected;
        public event Action Produced;

        public PickableCollectorMultipleConditionRuleset Ruleset;

        [SerializeField] Transform _modifiedStockpilePoint;
        [SerializeField] Transform UnmodifyTransform;
        [SerializeField] Transform UnmodifyTransformSecondProduct;
        [SerializeField] Transform ModifyTransform;
        [SerializeField] Animator machineAnimator;
        [SerializeField, Range(0f, 20f)] float _stockpileEveryXSec;
        [SerializeField, Range(0f, 20f)] float _jumpHeight;

        Dictionary<PickablePoolerSO, int> _neededResources;

        [SerializeField] int amountToProduce = 0;
        void Awake()
        {
            spawnedPickables = new Stack<Pickable>();
            _neededResources = new Dictionary<PickablePoolerSO, int>();
            foreach (TypeCountPair rulesetTypeCountPair in Ruleset.TypeCountPairs)
            {
                _neededResources.Add(rulesetTypeCountPair.PickablePooler, rulesetTypeCountPair.Count);
            }
        }

        public override PickablePoolerSO GetPool()
        {
            return Ruleset.OutputPooler;
        }

        protected override IEnumerator CollectFromPlayer(InventoryManager inventoryManager)
        {
            while (true)
            {
                if (GetNeededPickable(inventoryManager, out PickablePoolerSO neededPickablePool))
                {
                    if (inventoryManager.TakePickable(neededPickablePool, out Pickable pickable))
                    {
                        Debug.Log("Collecting");
                        pickable.MoveThenDissappearSlowlyToPool(ChooseUnModifyTransform(pickable), .1f);
                        _neededResources[pickable.Pool] -= 1;
                        Collected?.Invoke(neededPickablePool, _neededResources[pickable.Pool]);

                        // Проверяем, собраны ли все необходимые ресурсы
                        if (CheckAllResourcesCollected())
                        {
                            amountToProduce++;
                            // Подготавливаем анимацию и состояние машины к следующему циклу сс
                            machineAnimator.SetBool("canProduce", true);
                        }
                    }
                }
                yield return null;
            }
        }

        bool CheckAllResourcesCollected()
        {
            foreach (var resource in _neededResources)
            {
                if (resource.Value > 0) return false;
            }
            return true;
        }

        void ResetNeededResources()
        {
            foreach (TypeCountPair rulesetTypeCountPair in Ruleset.TypeCountPairs)
            {
                _neededResources[rulesetTypeCountPair.PickablePooler] = rulesetTypeCountPair.Count;
            }
        }

        bool GetNeededPickable(InventoryManager inventoryBase, out PickablePoolerSO poolerSo)
        {
            foreach (var neededResource in _neededResources)
            {
                if (neededResource.Value > 0 && inventoryBase.ContainsPickable(neededResource.Key))
                {
                    poolerSo = neededResource.Key;
                    return true;
                }
            }

            poolerSo = null;
            return false;
        }

        private Transform ChooseUnModifyTransform(Pickable pickable)
        {
            switch (pickable.PickableTypes)
            {
                case PickableTypes.CHOCALATEPACKAGE:
                    return UnmodifyTransform;
                case PickableTypes.HAZELNUT:
                    return UnmodifyTransformSecondProduct;
            }

            return null;
        }
        public void ProduceOutput()
        {
            if (amountToProduce > 0)
            {
                Pickable p = Ruleset.OutputPooler.TakeFromPool();
                p.transform.position = ModifyTransform.position;
                spawnedPickables.Push(p);
                Produced?.Invoke();
                amountToProduce--;

                // Проверяем, нужно ли продолжать производство или сбросить состояние
                if (amountToProduce <= 0)
                {
                    machineAnimator.SetBool("canProduce", false);
                    // Сбрасываем состояние для следующего цикла производства
                    ResetForNextCycle();
                }
            }
        }
        void ResetForNextCycle()
        {
            // Сбрасываем необходимые ресурсы и любые другие состояния, необходимые для начала нового цикла
            ResetNeededResources();
            // Можете добавить здесь любые дополнительные действия для сброса состояний
        }

    }
}