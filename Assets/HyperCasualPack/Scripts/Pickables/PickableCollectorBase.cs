using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HyperCasualPack.Pools;
using UnityEngine;

namespace HyperCasualPack.Pickables
{
    public abstract class PickableCollectorBase : MonoBehaviour
    {
        [SerializeField] protected RowColumnHeight RowColumnHeight;
        [SerializeField, Range(0.01f, 2f)] protected float JumpDuration;

        public Stack<Pickable> spawnedPickables;
        protected Coroutine _cor;
        
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out InventoryManager inventory))
            {
                _cor = StartCoroutine(CollectFromPlayer(inventory));
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out InventoryManager _))
            {
                StopCoroutine(_cor);
            }
        }

        protected abstract IEnumerator CollectFromPlayer(InventoryManager inventoryManager);

        protected void JumpOrganized(Pickable pickableItem, Transform pivotPoint, int index)
        {
            JumpOrganizedData jumpOrganizedData = new JumpOrganizedData
            {
                Duration = JumpDuration,
                Index = index,
                Item = pickableItem.transform,
                RowColumnHeight = RowColumnHeight,
                PivotPoint = pivotPoint
            };
            ArcadeIdleHelper.JumpOrganized(jumpOrganizedData);
        }
        protected void JumpOrganizedPrePos(Pickable pickableItem, Transform pivotPoint, int index,Transform prepos)
        {
            JumpOrganizedData jumpOrganizedData = new JumpOrganizedData
            {
                Duration = JumpDuration,
                Index = index,
                Item = pickableItem.transform,
                RowColumnHeight = RowColumnHeight,
                PivotPoint = pivotPoint
            };
            ArcadeIdleHelper.JumpOrganizedPrePositionSet(jumpOrganizedData,prepos);
        }

        public abstract PickablePoolerSO GetPool();

        public Pickable GetPickable()
        {
            return spawnedPickables.Count > 0 ? spawnedPickables.Pop() : null;
        }
    }
}