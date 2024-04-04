using System;
using HyperCasualPack.Pools;

namespace HyperCasualPack.Pickables
{
    [Serializable]
    public struct TypeCountPair
    {
        public PickablePoolerSO PickablePooler;
        public int Count;
    }
}