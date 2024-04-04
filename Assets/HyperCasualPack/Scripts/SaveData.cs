using System;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualPack
{
	class SaveData : ISerializationCallbackReceiver
	{
		// Keys and values should stay public and not read only for serialization purpose
		public List<string> keys = new List<string>();
		public List<int> values = new List<int>();
		public Dictionary<string, int> saves = new Dictionary<string, int>();
		
		public void OnBeforeSerialize()
		{
			keys.Clear();
			values.Clear();

			foreach (var kvp in saves)
			{
				keys.Add(kvp.Key);
				values.Add(kvp.Value);
			}
		}

		public void OnAfterDeserialize()
		{
			saves = new Dictionary<string, int>();

			for (int i = 0; i != Math.Min(keys.Count, values.Count); i++)
			{
				saves.Add(keys[i], values[i]);
			}
		}

	}
}
