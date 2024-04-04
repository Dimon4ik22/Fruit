using HyperCasualPack.Pools;

namespace HyperCasualPack.Pickables
{
    public interface IPickableCollectable
    {
        PickablePoolerSO GetPool();
        Pickable Collect();
    }
}