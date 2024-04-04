using HyperCasualPack.Pickables;
using UnityEngine;


public class FactoryMultipleCondition : MonoBehaviour
{
    [SerializeField] PickableCollectorMultipleCondition _pickableCollectorStockpiler;

    public void ModifyItem()
    {
        _pickableCollectorStockpiler.ProduceOutput();
    }
}