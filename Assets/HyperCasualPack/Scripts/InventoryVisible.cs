using System;
using DG.Tweening;
using HyperCasualPack.Pickables;
using HyperCasualPack.Pools;
using UnityEngine;

namespace HyperCasualPack
{
	[Serializable]
	public class InventoryVisible : InventoryBase
	{
		[SerializeField] public RowColumnHeight _rowColumnHeight;
		[SerializeField] private MaxTextController _maxTextController;
		public bool isMainPlayer = false;
		protected override void MovePickable(Pickable pickable)
		{
			if(isMainPlayer)
				Sounds.Instance.PlayPickup();
			Vector3 targetPos = ArcadeIdleHelper.GetPoint(pickableIndex, _rowColumnHeight);
			Transform trans = pickable.transform;
			trans.DOKill();
			trans.SetParent(stackingPoint);
			trans.DOLocalRotate(Vector3.zero, 0.5f).SetRecyclable();
			trans.DOLocalMove(targetPos, 0.5f).SetEase(Ease.InBack, 2f).SetRecyclable();
		}

		public bool IsInventoryEmpty()
		{
			if (pickableIndex == 0)
				return true;
			
			return false;
		}
		public Transform ReturnStackPoint()
		{
			return base.stackingPoint;
		}
		protected override bool IsCapacityFull(PickablePoolerSO p)
		{
			return pickableIndex >= _rowColumnHeight.GetCapacity();
		}

		public void ChangeRowColumnHeight(float value)
		{
			_rowColumnHeight.HeightOffset = value;
		}
	}
}
