using System;
using System.Collections.Generic;
using HyperCasualPack.Pickables;
using HyperCasualPack.Pools;
using UnityEngine;

namespace HyperCasualPack
{
    [Serializable]
    public abstract class InventoryBase
    {
        [SerializeField] protected Transform stackingPoint;

        protected Dictionary<PickablePoolerSO, Stack<Pickable>> Pickables { get; set; } = new Dictionary<PickablePoolerSO, Stack<Pickable>>();
        protected int pickableIndex;

        public bool IsEmpty()
        {
            return pickableIndex <= 0;
        }
        
        public bool CanTakePickable(PickablePoolerSO pool)
        {
            AddKey(pool);
            return !IsCapacityFull(pool);
        }
        

        public bool TakeRandomPickable(out Pickable pickable)
        {
            foreach (var pickable1 in Pickables)
            {
                if (pickable1.Value.Count > 0)
                {
                    TakePickableInstantly(out pickable, pickable1.Value);
                    return true;
                }
            }

            pickable = null;
            return false;
        }
        void TakePickableInstantly(out Pickable pickable, Stack<Pickable> pickableStack)
        {
            pickable = pickableStack.Pop();
            pickable.transform.SetParent(null);
            pickable.gameObject.SetActive(true);
            pickableIndex--;
        }

        public bool TakePickable(PickablePoolerSO p, out Pickable pickable)
        {
            if (!Pickables.ContainsKey(p))
            {
                pickable = null;
                return false;
            }
            
            if (Pickables[p].Count > 0)
            {
                TakePickableInstantly(out pickable, Pickables[p]);
                return true;
            }

            pickable = null;
            return false;
        }

        public bool ContainsPickable(PickablePoolerSO pool)
        {
            if (!Pickables.ContainsKey(pool))
            {
                return false;
            }
            
            if (Pickables[pool].Count > 0)
            {
                return true;
            }

            return false;
        }

        public void AddPickable(Pickable p)
        {
            AddKey(p.Pool);
            if (IsCapacityFull(p.Pool))
            {
                return;
            }

            pickableIndex++;
            MovePickable(p);
            Pickables[p.Pool].Push(p);
        }
        
        protected abstract void MovePickable(Pickable pickable);
        protected abstract bool IsCapacityFull(PickablePoolerSO p);
        
        void AddKey(PickablePoolerSO p)
        {
            if (Pickables.ContainsKey(p))
            {
                return;
            }

            Pickables.Add(p, new Stack<Pickable>());
        }
    }
}