using System;
using UnityEditor;
using UnityEngine;

namespace HyperCasualPack.ScriptableObjects
{
    public abstract class SaveableSO : ScriptableObject
    {
        [SerializeField, HideInInspector]  string _guid;

        public string GetGuid => _guid;

        public abstract object GetDefaultValue { get; }

#if UNITY_EDITOR
        void OnValidate()
        {
            string path = AssetDatabase.GetAssetPath(this);
            _guid = AssetDatabase.AssetPathToGUID(path);
        }
#endif

        public event Action Loaded;

        public abstract void RestoreState(object obj);

        public abstract object CaptureState();

        public void OnLoaded()
        {
            Loaded?.Invoke();
        }
    }
}