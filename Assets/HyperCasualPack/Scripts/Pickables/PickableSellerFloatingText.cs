using DG.Tweening;
using HyperCasualPack.Pools;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HyperCasualPack.Pickables
{
	public class PickableSellerFloatingText : PickableSellerBase
	{
		[SerializeField] TextPoolerSO _textPooler;
		[SerializeField, Range(0f, 20f)] float _feedbackXRandomRange;
		[SerializeField, Range(0f, 30f)] float _feedbackUpMovementDistance;
		[SerializeField, Range(0f, 10f)] float _feedbackShownDuration;

		Camera _camera;

		void Start()
		{
			_camera = Camera.main;
		}

		protected override void OnJump(Pickable pickable)
		{
			int pickableSellValue = pickable.GetSellValue();
			pickable.ReleasePool();
			IncomeResource.RuntimeValue += pickableSellValue;
			TextMeshPro txt = _textPooler.TakeFromPool();
			txt.text = pickableSellValue.ToString();
			txt.transform.position = transform.position + Vector3.up;
			Transform camTrans = _camera.transform;
			txt.transform.LookAt(camTrans.position + camTrans.forward * 500f);
			float rndX = Random.Range(-_feedbackXRandomRange, _feedbackXRandomRange);
			txt.transform.DOMove(txt.transform.position + Vector3.up * _feedbackUpMovementDistance + transform.right * rndX, _feedbackShownDuration)
				.SetRecyclable()
				.OnComplete(() => _textPooler.PutBackToPool(txt));
		}
	}
}
