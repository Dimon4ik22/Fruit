using System;
using System.Collections;
using DG.Tweening;
using HyperCasualPack.Pools;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HyperCasualPack
{
    public class Unlocker : MonoBehaviour
    {
        [SerializeField] public UnityEvent _onUnlocked;

        [SerializeField] TextMeshProUGUI _resourceCountText;
        [SerializeField] Image _progressBar;
        [SerializeField] PickableResourcePoolerSO _neededResource;
        [SerializeField] Ease _spendingEase;
        [SerializeField, Range(0f, 100f)] float _spendingSpeed;
        [SerializeField, Range(0, 99999)] int _requiredResource;

        InventoryManager _inventoryManager;
        ResourceSpender _resourceSpender;
        Tween _spendingTween;
        Coroutine _cor;
        WaitForSeconds _waitForSeconds = new WaitForSeconds(0.1f);
        int _lastSpentResourceAmount;
        int _collectedResource;
        bool _canWork = true;
        private bool hasUnlocked = false;
        [Required]
        public string UnlockerTutorialDescription;

        void OnDisable()
        {
            _onUnlocked.RemoveListener(StopSound);
        }

        void OnEnable()
        {
            _onUnlocked.AddListener(StopSound);
        }

        void StopSound()
        {
            Sounds.Instance.StopSound();
        }

        void OnTriggerEnter(Collider other)
        {
            if (!_canWork)
                return;

            if (other.TryGetComponent(out InventoryManager inventory))
            {
                _inventoryManager = inventory;
                _cor = StartCoroutine(CheckInventory(other));
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (!_canWork)
                return;

            if (other.TryGetComponent(out InventoryManager _))
            {
                StopSound();
                _spendingTween.Kill();
                _inventoryManager = null;
                _resourceSpender = null;
                StopCoroutine(_cor);
            }
        }

        void OnValidate()
        {
            _resourceCountText.text = _requiredResource.ToString();
        }

        IEnumerator CheckInventory(Collider other)
        {
            while (true)
            {
                if (!_inventoryManager.IsInteractable())
                {
                    yield return _waitForSeconds;
                    continue;
                }

                if (other.TryGetComponent(out ResourceSpender resourceSpender))
                {
                    _resourceSpender = resourceSpender;
                    int resourceAmount = _resourceSpender.GetRuntimeIntValue(_neededResource);
                    if(resourceAmount >= _requiredResource)
                        ArcadeIdleHelper.SpendResource(_requiredResource, _collectedResource, resourceAmount, out _spendingTween, _spendingSpeed, _spendingEase, SpendMoney);

                    yield break;
                }

                yield return _waitForSeconds;
            }
        }

        void SpendMoney(int x)
        {
            Sounds.Instance.PlaySpendMoney();
            int decreasingAmountDelta = x - _lastSpentResourceAmount;
            _resourceSpender.Spend(_neededResource, decreasingAmountDelta, transform);
            _collectedResource += decreasingAmountDelta;
            _resourceCountText.text = (_requiredResource - _collectedResource).ToString();
            _lastSpentResourceAmount = x;
            _progressBar.fillAmount = (float)_collectedResource / _requiredResource;
            if (_collectedResource == _requiredResource)
            {
                if (!hasUnlocked)
                    _onUnlocked?.Invoke();
                hasUnlocked = true;
                _canWork = false;
                StopCoroutine(_cor);
            }
        }
    }

    public class UnlockerTutorial
    {
        public string TutorialDescription;
    }
    
}