using System.Collections;
using System.Collections.Generic;
using HyperCasualPack;
using HyperCasualPack.Pickables;
using UnityEngine;

public class PickableTreeCollector : MonoBehaviour
{
  
        [SerializeField] PickableCollectorBase _pickableCollectorBase;
        [SerializeField, Range(0f, 20f)] float _pickTime;

        WaitForSeconds _waitForSeconds;
        InventoryManager _inventoryManager;
        Coroutine _coroutine;

        void Awake()
        {
            _waitForSeconds = new WaitForSeconds(_pickTime);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out InventoryManager inventoryManager))
            {
                _coroutine = StartCoroutine(Co_Collect(inventoryManager));
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out InventoryManager inventoryManager))
            {
                StopCoroutine(_coroutine);
            }
        }

        IEnumerator Co_Collect(InventoryManager inventory)
        {
            while (true)
            {
                if (!inventory.CanTakePickable(_pickableCollectorBase.GetPool()))
                {
                    yield return null;
                    continue;
                }
                    
                Pickable pickable = _pickableCollectorBase.GetPickable();
                if (pickable && !pickable.isPickableSeed)
                {
                    var pickableProduct = pickable.GetComponent<PickableProduct>();
                    pickableProduct?.DisablePhyiscs();
                    inventory.AddPickable(pickable);
                }
                else if (pickable && pickable.isPickableSeed)
                {
                    inventory.AddPickable(pickable);
                    pickable.SeedSlot.isAvailable = true;
                }
                
                yield return _waitForSeconds;
            }
        }

        public void GiveToAI(InventoryManager inventory)
        {
            
            if (inventory.CanTakePickable(_pickableCollectorBase.GetPool()))
            {
                Pickable pickable = _pickableCollectorBase.GetPickable();
                inventory.AddPickable(pickable);
                pickable.SeedSlot.isAvailable = true;

            }
        }
    
}
