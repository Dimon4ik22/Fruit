using System;
using HyperCasualPack.Pickables;
using HyperCasualPack.Pools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HyperCasualPack
{
	public class InventoryManager : MonoBehaviour
	{
		[SerializeField] ArcadeIdleMover _arcadeIdleMover;
		
		[SerializeField] InventoryInvisible _inventoryInvisible;
		[SerializeField] InventoryVisible _inventoryVisible;
		[SerializeField] private Animator _animator;
		PickableTypes lastEnteredPickableType;
		public bool IsInteractable()
		{
			return _arcadeIdleMover.IsStopped;
		}

		private void Update()
		{
			if(_inventoryVisible.IsEmpty())
				_animator.SetBool("Carrying",false);
		}

		public bool IsInventoryEmpty()
		{
			return _inventoryVisible.IsInventoryEmpty();
		}
		public Transform ReturnStackPoint()
		{
			return _inventoryVisible.ReturnStackPoint();
		}

		public void ChangeHeightOffset(float val)
		{
			_inventoryVisible.ChangeRowColumnHeight(val);
		}

		public void ReturnFirstPickableInTheInventory(Pickable p)
		{
			_inventoryVisible.TakeRandomPickable(out p);
		}
		public void IncrementHeightCount()
		{
			_inventoryVisible._rowColumnHeight.HeightCount++;
		}

		public void AddPickable(Pickable pickable)
		{
			if (pickable.PickableData.IsVisible)
			{
				
				// ChangeHeightOffset(pickable.PickableOffsetSo.heightOffset);
				//en son giren pickabletype ı burada tut her girişte aynı pickable mı girmis kontrol et
				_inventoryVisible.AddPickable(pickable);
				_animator.SetBool("Carrying",true);

			}
			else
			{
				_inventoryInvisible.AddPickable(pickable);
			}
		}
		
		public bool CanTakePickable(PickablePoolerSO pool)
		{
			if (!_arcadeIdleMover.IsStopped)
			{
				return false;
			}

			return pool.PeekData().IsVisible ? _inventoryVisible.CanTakePickable(pool) : _inventoryInvisible.CanTakePickable(pool);
		}

		public bool TakePickable(PickablePoolerSO p, out Pickable pickable)
		{
			if (p.PeekData().IsVisible)
			{
				//Sounds.Instance.PlayPickup();
				return _inventoryVisible.TakePickable(p, out pickable);
			}
			
			return _inventoryInvisible.TakePickable(p, out pickable);
		}

		public bool ContainsPickable(PickablePoolerSO poolerSO)
		{
			return poolerSO.PeekData().IsVisible ? _inventoryVisible.ContainsPickable(poolerSO) : _inventoryInvisible.ContainsPickable(poolerSO);
		}

		public bool TakeRandomInvisiblePickable(out Pickable pickable)
		{
			return _inventoryInvisible.TakeRandomPickable(out pickable);
		}
		
		public bool TakeRandomVisiblePickable(out Pickable pickable)
		{
			return _inventoryVisible.TakeRandomPickable(out pickable);
		}

		public bool TakeRandomPickable(out Pickable pickable)
		{
			if (!_inventoryVisible.IsEmpty() && !_inventoryInvisible.IsEmpty())
			{
				if (Random.value > 0.5f)
				{
					return TakeRandomInvisiblePickable(out pickable);
				}
			
				return TakeRandomVisiblePickable(out pickable);
			}
			if (!_inventoryVisible.IsEmpty())
			{
				return TakeRandomVisiblePickable(out pickable);
			}
			if (!_inventoryInvisible.IsEmpty())
			{
				return TakeRandomInvisiblePickable(out pickable);
			}

			pickable = null;
			return false;
		}
	}
}
