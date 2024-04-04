using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualPack.Pools
{
    public abstract class ObjectPoolerSO<T> : ScriptableObject where T : MonoBehaviour
    {
        [SerializeField] protected int _maxSize;
        [SerializeField] protected T _behaviour;
        protected Queue<T> pooledObjectQueue;

        public virtual T TakeFromPool()
        {
            pooledObjectQueue ??= new Queue<T>();

            T obj;
            if (pooledObjectQueue.Count > 0)
            {
                obj = pooledObjectQueue.Dequeue();
                obj.gameObject.SetActive(true);
            }
            else
            {
                obj = Instantiate(_behaviour);
            }

            return obj;
        }

        public void PutBackToPool(T t)
        {
            if (pooledObjectQueue.Count > _maxSize)
            {
                Destroy(t.gameObject);
            }
            else
            {
                t.gameObject.SetActive(false);
                pooledObjectQueue.Enqueue(t);
            }
        }
    }
}