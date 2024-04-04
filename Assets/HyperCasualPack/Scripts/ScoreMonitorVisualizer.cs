using HyperCasualPack.Pools;
using HyperCasualPack.ScriptableObjects;
using UnityEngine;

namespace HyperCasualPack
{
	[CreateAssetMenu(menuName = "SO/ScoreMonitorVisualizer", fileName = "ScoreMonitorVisualizer", order = 0)]
	public class ScoreMonitorVisualizer : ScriptableObject
	{
		[SerializeField] ScoreMovablePoolerSO _scoreMovablePool;
		[SerializeField] RuntimeResourceUIVariable _scoreUIParentObject;
		[SerializeField] SaveableRuntimeIntVariable _resource;
		[SerializeField, Range(0f, 5f)] float _visualizationDuration;

		Camera _camMain;

		public void PlayVisualization(Vector3 pickedItemWorldPos, int resourceAmount)
		{
			if (!_camMain) _camMain = Camera.main;

			ScoreMovable scoreMovableObject = _scoreMovablePool.TakeFromPool();
			scoreMovableObject.PlayMoveSequenceFromWorldToUI(_scoreUIParentObject, _camMain, pickedItemWorldPos,
				_visualizationDuration, _scoreMovablePool, OnMoveSequenceEnded, resourceAmount);
		}

		void OnMoveSequenceEnded(int resourceAmount)
		{
			_resource.RuntimeValue += resourceAmount;
		}
	}
}
