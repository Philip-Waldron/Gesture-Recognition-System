using UnityEngine;

namespace Gesture_Recognition_System.Scripts.Utils
{
	// Singleton behaviour class, used for components that should only have one instance
	[ExecuteInEditMode]
	public class Singleton<T> : MonoBehaviour where T : Singleton<T>
	{
		// Prevent access to instance when moving between play and edit modes, or when object is disabled.
		private static bool _lockOut = false;
		
		private static T _instance;

		public static T Instance
		{
			get
			{
				if (_lockOut)
				{
					Debug.LogWarning("Singleton " + typeof(T).Name + " is disabled, returning null - ignore on recompile or entering playmode");
					return null;
				}
				
				if(_instance == null)
				{
					_instance = FindObjectOfType<T>();
					if (_instance == null)
					{
						Debug.LogErrorFormat("No instances of singleton " + typeof(T).Name + " found, please add one");
						return null;
					}
				}

				return _instance;
			}
		}

		protected virtual void Awake()
		{
			if (FindObjectsOfType<T>().Length > 1)
			{
				Debug.LogErrorFormat("Tried to add an instance of singleton " + typeof(T).Name + " when there is one already in the scene!");
				DestroyImmediate(this);
			}
		}

		protected virtual void OnDisable()
		{
			_lockOut = true;
		}
		
		protected virtual void OnEnable()
		{
			_lockOut = false;
			
			if(_instance == null)
			{
				InitInstance();
			}
		}

		private static void InitInstance()
		{
			_instance = FindObjectOfType<T>();
			if (_instance == null)
			{
				print("No instances of singleton " + typeof(T).Name + " found, please add one");
				//_instance = (new GameObject(typeof(T).Name)).AddComponent<T>();
				// Currently adding a new gameobject with a singleton that operates in both edit and
				// play mode causes a broken and buggy instance to be created in between those modes
				// that will persist. Uncomment and play scene with missing singleton, then run
				// SingletonDetector to find broken instance of the singleton.
			}
		}
	}
}