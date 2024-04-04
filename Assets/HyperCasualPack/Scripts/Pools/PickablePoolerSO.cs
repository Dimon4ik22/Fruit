using System.Collections.Generic;
using HyperCasualPack.Pickables;
using UnityEngine;

namespace HyperCasualPack.Pools
{
    [CreateAssetMenu(menuName = "HyperCasualPack/Pools/Pickable Pooler", fileName = "Pickable Pooler", order = 0)]
    public class PickablePoolerSO : ObjectPoolerSO<Pickable>
    {
        public override Pickable TakeFromPool()
        {
            pooledObjectQueue ??= new Queue<Pickable>();

            Pickable obj;
            if (pooledObjectQueue.Count > 0)
            {
                obj = pooledObjectQueue.Dequeue();
                obj.gameObject.SetActive(true);
            }
            else
            {
                obj = Instantiate(_behaviour);
                obj.Pool = this;
            }

            return obj;
        }

        public PickableData PeekData()
        {
            return _behaviour.PickableData;
        }
    }
}