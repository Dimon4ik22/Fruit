using System.Collections;
using HyperCasualPack.ScriptableObjects;
using UnityEngine;

namespace HyperCasualPack.Pickables
{
	public abstract class PickableSellerBase : MonoBehaviour
	{
		[SerializeField] protected SaveableRuntimeIntVariable IncomeResource;
		[SerializeField, Tooltip("Selling jupm duration"), Range(0f, 10f)] float _jumpDuration;
		[SerializeField, Tooltip("Sell jump height"), Range(0f, 10f)] float _jumpHeight;
		[SerializeField, Tooltip("Sell count per second"), Range(0f, 20f)] float _sellRate;
		
		Coroutine _cor;
		WaitForSeconds _waitForSeconds;

		void Awake()
		{
			_waitForSeconds = new WaitForSeconds(1f / _sellRate);
		}

		void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out InventoryManager inventory))
			{
				_cor = StartCoroutine(CollectFromPlayer(inventory));
			}
		}

		void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out InventoryManager _) && _cor != null)
			{
				StopCoroutine(_cor);
			}
		}

		IEnumerator CollectFromPlayer(InventoryManager inventoryManager)
		{
			while (inventoryManager.TakeRandomPickable(out Pickable pickable))
			{
				PlaySequence(pickable);
				yield return _waitForSeconds;
			}
		}
		
		void PlaySequence(Pickable pickableItem)
        {
            pickableItem.transform.Jump(transform, _jumpHeight, 1, _jumpDuration, () => OnJump(pickableItem));
        }

		protected abstract void OnJump(Pickable pickable);
	}

}
