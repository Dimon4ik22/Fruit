using HyperCasualPack.ScriptableObjects;
using Sirenix.OdinInspector;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

namespace HyperCasualPack
{
	public class IntVariableMonitor : MonoBehaviour
	{
        [DllImport("__Internal")]
        private static extern void AddScoreExtern(int value);

        [SerializeField] SaveableRuntimeIntVariable _monitorVariable;
		[SerializeField] TextMeshProUGUI _monitorText;

		void OnEnable()
		{
			_monitorVariable.ValueChanged += MonitorVariableOnValueChanged;
		}

        void OnDisable()
		{
			_monitorVariable.ValueChanged -= MonitorVariableOnValueChanged;
		}
		
		void MonitorVariableOnValueChanged(int obj)
		{
			_monitorText.text = obj.ToString();
            _monitorVariable.CaptureState();
        }

		[Button]
		private void SetGoldZero()
		{
			_monitorVariable.RuntimeValue = 0;
		}
        [Button]
        private void SetGoldPlus1000()
        {
            _monitorVariable.RuntimeValue += 1000;
        }

		public void ShowAdvButton()
		{
			Time.timeScale = 0;
			AddScoreExtern(100);
        }

        public void AddScore(int value)
        {
            Debug.Log("AddScore call");
            _monitorVariable.RuntimeValue += value;
            _monitorVariable.CaptureState();
        }
    }
}
