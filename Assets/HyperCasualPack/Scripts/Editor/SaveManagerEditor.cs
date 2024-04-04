using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace HyperCasualPack.Scripts.Editor
{
	public static class SaveManagerEditor
	{
		[MenuItem("Tools/HypercasualPack/Open Save Directory")]
		static void OpenSaveDirectory()
		{
			string savePath = Application.persistentDataPath;

			if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				System.Diagnostics.Process.Start("open", $"-R \"{savePath}\"");
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				Application.OpenURL(savePath);
			}
		}
	}
}
