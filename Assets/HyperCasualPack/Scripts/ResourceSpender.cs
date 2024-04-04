using System;
using System.Collections.Generic;
using HyperCasualPack.Pickables;
using HyperCasualPack.Pools;
using HyperCasualPack.ScriptableObjects;
using UnityEngine;

namespace HyperCasualPack
{
	public class ResourceSpender : MonoBehaviour
	{
		[SerializeField] ResourceSpenderData[] _resourceSpenderDatas;
		[SerializeField, Tooltip("spawning resource visuals every X resource spent"), Range(0, 100)] int _visualFeedbackSpawnRate;
		[SerializeField, Range(0f, 10f)] float _jumpHeight;
		[SerializeField, Range(0f, 3f)] float _jumpDuration;

		Dictionary<PickableResourcePoolerSO, SaveableRuntimeIntVariable> _spendableResources;
		int _spawnCount;

		void Awake()
		{
			_spendableResources = new Dictionary<PickableResourcePoolerSO, SaveableRuntimeIntVariable>();
			foreach (ResourceSpenderData resourceSpenderData in _resourceSpenderDatas)
			{
				_spendableResources.Add(resourceSpenderData.SpendingResourcePool, resourceSpenderData.ResourceVariable);
			}
		}

		public void Spend(PickableResourcePoolerSO pickableResource, int amount, Transform moneyTargetPoint)
		{
			_spendableResources[pickableResource].RuntimeValue -= amount;
			_spawnCount += amount;
			if (_spawnCount >= _visualFeedbackSpawnRate)
			{
				PickableResource nonPickableResource = pickableResource.TakeFromPool();
				nonPickableResource.transform.position = transform.position;
				nonPickableResource.transform.Jump(moneyTargetPoint, _jumpHeight, 1, _jumpDuration, () =>
				{
					pickableResource.PutBackToPool(nonPickableResource);
				});
				_spawnCount = 0;
			}
		}

		public int GetRuntimeIntValue(PickableResourcePoolerSO p)
		{
			return _spendableResources[p].RuntimeValue;
		}
	}

	[Serializable]
	struct ResourceSpenderData
	{
		public PickableResourcePoolerSO SpendingResourcePool;
		public SaveableRuntimeIntVariable ResourceVariable;
	}
}
