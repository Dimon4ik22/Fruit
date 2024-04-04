using HyperCasualPack.Pools;
using UnityEngine;

namespace HyperCasualPack.Pickables
{
    [CreateAssetMenu(fileName = "PickableCollectorMultipleConditionRuleset", menuName = "HyperCasualPack/Pickables/Pickable Collector Multiple Condition Ruleset", order = 0)]
    public class PickableCollectorMultipleConditionRuleset : ScriptableObject
    {
        public TypeCountPair[] TypeCountPairs;
        public PickablePoolerSO OutputPooler;

        public bool Contains(PickablePoolerSO p)
        {
            foreach (TypeCountPair typeCountPair in TypeCountPairs)
            {
                if (typeCountPair.PickablePooler == p)
                {
                    return true;
                }
            }

            return false;
        }
    }
}