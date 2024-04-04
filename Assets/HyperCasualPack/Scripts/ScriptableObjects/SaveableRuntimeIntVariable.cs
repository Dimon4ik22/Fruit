using System;
using UnityEngine;

namespace HyperCasualPack.ScriptableObjects
{
    [CreateAssetMenu(menuName = "HyperCasualPack/Variables/Saveable Runtime Int Variable", fileName = "Saveable Runtime Int Variable", order = 0)]
    public class SaveableRuntimeIntVariable : SaveableSO
    {
        [SerializeField] int _initialValue;
        int _runtimeValue = 689;

        private const string PlayerPrefsKey = "SaveableRuntimeIntVariable";

        public int RuntimeValue
        {
            get => _runtimeValue;
            set
            {
                _runtimeValue = value;
                ValueChanged?.Invoke(_runtimeValue);
            }
        }

        public override object GetDefaultValue => _initialValue;

        public event Action<int> ValueChanged;

        public override void RestoreState(object obj)
        {
            RuntimeValue = PlayerPrefs.GetInt(PlayerPrefsKey, (int)obj);
        }

        public override object CaptureState()
        {
            PlayerPrefs.SetInt(PlayerPrefsKey, RuntimeValue);
            return RuntimeValue;
        }
    }
}