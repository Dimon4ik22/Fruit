using UnityEngine;

namespace HyperCasualPack.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Movement Data", menuName = "HyperCasualPack/Movement Data", order = 0)]
    public class MovementDataSO : ScriptableObject
    {
        public float SideMoveSpeed;
        public float ForwardMoveSpeed;
    }
}