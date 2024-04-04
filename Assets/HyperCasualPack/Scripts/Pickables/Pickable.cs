using HyperCasualPack.Pools;
using UnityEngine;

namespace HyperCasualPack.Pickables
{
    public enum PickableTypes
    {
        COCAOSEED,
        CHOCALATEPACKAGE,
        HERSHEYKISSES,
        HAZELNUT,
        FERREROROCHER,
        CHOCALATECOOKIE,
        MMCHOCOLATE
    }
    public class Pickable : MonoBehaviour
    {
        [SerializeField] PickableData _pickableData;
        Vector3 _defaultLocalScale;
        public bool isPickableSeed = false;
        public SeedSlot SeedSlot;
        public PickablePoolerSO Pool { get; set; }
        public PickableTypes PickableTypes;
        public PickableOffsetSO PickableOffsetSo;
        public PickableData PickableData => _pickableData;
        public Vector3 DefaultScale => _defaultLocalScale;
        
        void Awake()
        {
            _defaultLocalScale = transform.localScale;
        }

        public int GetSellValue()
        {
            return _pickableData.SellValue;
        }

        public void ReleasePool()
        {
            ResetState();
            Pool.PutBackToPool(this);
        }

        void ResetState()
        {
            transform.localScale = _defaultLocalScale;
            transform.Clear();
        }
    }
}