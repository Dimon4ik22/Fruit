using HyperCasualPack.Pools;
using UnityEngine;

namespace HyperCasualPack.Pickables
{
    [CreateAssetMenu(fileName = "PickableCollectorStockpilerRuleset", menuName = "HyperCasualPack/Pickables/Pickable Collector Stockpiler Ruleset", order = 0)]
    public class PickableCollectorStockpilerRuleset : ScriptableObject
    {
        public PickablePoolerSO InputPoolerSO;
        public PickablePoolerSO OutputPoolerSO;
    }
}