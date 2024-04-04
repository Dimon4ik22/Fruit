using System;
using DG.Tweening;
using HyperCasualPack.Pickables;
using HyperCasualPack.Pools;
using HyperCasualPack.ScriptableObjects;
using UnityEngine;

namespace HyperCasualPack
{
	[Serializable]
	public class InventoryInvisible : InventoryBase
	{
		[SerializeField] int _capacity;
		public SaveableRuntimeIntVariable money;
		public bool IsPlayer = false;
		protected override void MovePickable(Pickable pickable)
		{
			if(IsPlayer)
				Sounds.Instance.PlayPickup();
			if (pickable.transform.CompareTag("Money"))
			{
				money.RuntimeValue += 1;
			}
			Transform trans = pickable.transform;
			trans.DOKill();
			trans.SetParent(stackingPoint);
			trans.DOLocalRotate(Vector3.zero, 0.5f).SetRecyclable();
			trans.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.InBack, 2f).SetRecyclable().OnComplete(() =>
			{
				pickable.gameObject.SetActive(false);
			});
		}
		
		protected override bool IsCapacityFull(PickablePoolerSO p)
		{
			return pickableIndex >= _capacity;
		}
	}
}
