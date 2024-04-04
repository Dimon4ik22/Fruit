using System.Collections.Generic;
using HyperCasualPack.Pools;
using TMPro;
using UnityEngine;

namespace HyperCasualPack.Pickables
{
	public class MultipleConditionMonitor : MonoBehaviour
	{
		[SerializeField] PickableCollectorMultipleCondition _pickableCollectorMultipleCondition;
		[SerializeField] TMP_Text[] _itemCountTexts;
		
		Dictionary<PickablePoolerSO, TMP_Text> _pickableMonitors;

		void Awake()
		{
			_pickableMonitors = new Dictionary<PickablePoolerSO, TMP_Text>();

			if (_itemCountTexts.Length != _pickableCollectorMultipleCondition.Ruleset.TypeCountPairs.Length)
			{
				Debug.LogError("Not enough text fields to monitor! Add new text fields so monitor can show them");
				return;
			}
			
			for (int i = 0; i < _pickableCollectorMultipleCondition.Ruleset.TypeCountPairs.Length; i++)
			{
				TypeCountPair typeCountPair = _pickableCollectorMultipleCondition.Ruleset.TypeCountPairs[i];
				_pickableMonitors.Add(typeCountPair.PickablePooler, _itemCountTexts[i]);
				_itemCountTexts[i].text = typeCountPair.Count.ToString();
			}
		}

		void OnEnable()
		{
			_pickableCollectorMultipleCondition.Collected += PickableCollectorMultipleConditionOnCollected;
			_pickableCollectorMultipleCondition.Produced += PickableCollectorMultipleConditionOnProduced;
		}

		void OnDisable()
		{
			_pickableCollectorMultipleCondition.Collected -= PickableCollectorMultipleConditionOnCollected;
			_pickableCollectorMultipleCondition.Produced -= PickableCollectorMultipleConditionOnProduced;
		}

		void OnValidate()
		{
			_pickableCollectorMultipleCondition ??= GetComponentInParent<PickableCollectorMultipleCondition>();
		}

		void PickableCollectorMultipleConditionOnProduced()
		{
			ResetMonitors();
		}
		void ResetMonitors()
		{
			for (int i = 0; i < _pickableCollectorMultipleCondition.Ruleset.TypeCountPairs.Length; i++)
			{
				TypeCountPair pair = _pickableCollectorMultipleCondition.Ruleset.TypeCountPairs[i];
				_pickableMonitors[pair.PickablePooler].text = pair.Count.ToString();
			}
		}

		void PickableCollectorMultipleConditionOnCollected(PickablePoolerSO arg1, int arg2)
		{
			_pickableMonitors[arg1].text = arg2.ToString();
		}
	}
}
