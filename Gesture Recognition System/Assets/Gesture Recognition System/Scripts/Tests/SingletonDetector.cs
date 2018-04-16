using System.Collections.Generic;
using Gesture_Recognition_System.Scripts.Gestures;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gesture_Recognition_System.Scripts.Tests
{
	public class SingletonDetector : MonoBehaviour
	{
		public List<GestureManager> GestureManagers = new List<GestureManager>();
		
		[Button()]
		private void UpdateReferences()
		{
			GestureManagers.Clear();
			GestureManagers.AddRange(FindObjectsOfType<GestureManager>());
		}
		
		[Button()]
		private void KillObjects()
		{
			foreach (GestureManager bop in GestureManagers)
			{
				DestroyImmediate(bop);
			}
		}
	}
}
