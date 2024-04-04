using System.IO;
using UnityEngine;

namespace HyperCasualPack.ScriptableObjects
{
	public class SaveManager : MonoBehaviour
	{
		[SerializeField] SaveableSO[] _saveables;

		SaveData _saveData;
		static string savePath;

		void Awake()
		{
			savePath = Path.Combine(Application.persistentDataPath, "gamedata.json");
			_saveData = new SaveData();
		}

		void Start()
		{
			RestoreAll();
		}

		void OnApplicationPause(bool pauseStatus)
		{
			if (pauseStatus)
			{
				SaveAll();	
			}
		}

		void OnApplicationQuit()
		{
			SaveAll();
		}

		void RestoreAll()
		{
			if (File.Exists(savePath))
			{
				SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(savePath));
				
				foreach (SaveableSO saveableSO in _saveables)
				{
					saveableSO.RestoreState(saveData.saves[saveableSO.GetGuid]);
				}
			}
			else
			{
				foreach (SaveableSO saveableSO in _saveables)
				{
					saveableSO.RestoreState(saveableSO.GetDefaultValue);
				}
			}
		}

		void SaveAll()
		{
			foreach (SaveableSO saveableSO in _saveables)
			{
				if (_saveData.saves.ContainsKey(saveableSO.GetGuid))
				{
					_saveData.saves[saveableSO.GetGuid] = (int)saveableSO.CaptureState();
				}
				else
				{
					_saveData.saves.Add(saveableSO.GetGuid, (int)saveableSO.CaptureState());
				}
			}
			string convertedSaveData = JsonUtility.ToJson(_saveData);
			File.WriteAllText(savePath, convertedSaveData);
		}
	}

}
