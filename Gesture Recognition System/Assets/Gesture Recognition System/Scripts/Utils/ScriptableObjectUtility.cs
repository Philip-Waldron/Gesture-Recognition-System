using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace Gesture_Recognition_System.Scripts.Utils
{
	public static class ScriptableObjectUtility
	{
		// This makes it easy to create, name and place unique new ScriptableObject asset files.
		public static T CreateAsset<T>(string path, string fileName) where T : ScriptableObject
		{
			T asset = ScriptableObject.CreateInstance<T>();
			
			string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + fileName + ".asset");

			AssetDatabase.CreateAsset(asset, assetPathAndName);

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			EditorUtility.SetDirty(asset);
			//EditorUtility.FocusProjectWindow(); // Error with odin
			//Selection.activeObject = asset;
			return asset;
		}
	}
}
#endif