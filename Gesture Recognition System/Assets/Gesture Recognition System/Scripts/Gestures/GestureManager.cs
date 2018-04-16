using System.Collections.Generic;
using System.Linq;
using Gesture_Recognition_System.Scripts.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gesture_Recognition_System.Scripts.Gestures
{
	public class GestureManager : Singleton<GestureManager>
	{
		[SerializeField] [ListDrawerSettings(IsReadOnly = true)] [ReadOnly]
		private List<GestureObject> _gestureObjects = new List<GestureObject>();
		
		// prevent use of constructor to guarantee this will be a singleton
		protected GestureManager () {}

		public List<GestureObject> GetGestureObjects()
		{
			return _gestureObjects;
		}

		public GestureObject GetGestureObject(GestureObject gestureObject)
		{
			return _gestureObjects.Find(x => gestureObject);
		}

		public void RemoveGestureObject(GestureObject gestureObject)
		{
			_gestureObjects.Remove(gestureObject);
		}

		public void AddGestureObject(GestureObject gestureObject)
		{
			if (!_gestureObjects.Contains(gestureObject))
			{
				_gestureObjects.Add(gestureObject);
			}
		}

		private void Reset()
		{
			UpdateReferences();
		}

		[Button()]
		private void UpdateReferences()
		{
			_gestureObjects = FindObjectsOfType<GestureObject>().ToList();
		}
	}
}
