using UnityEngine;

namespace HyperCasualPack.Pickables
{
    [CreateAssetMenu(fileName = "Pickable Data", menuName = "HyperCasualPack/Pickable Data", order = 0)]
    public class PickableData : ScriptableObject
    {
        public bool IsVisible;
        public int SellValue;
    }
}