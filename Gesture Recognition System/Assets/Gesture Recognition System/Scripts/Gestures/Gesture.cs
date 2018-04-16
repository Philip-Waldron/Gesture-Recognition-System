using System;
using System.Collections.Generic;
using Gesture_Recognition_System.Scripts.Enums;
using UnityEngine;

namespace Gesture_Recognition_System.Scripts.Gestures
{
	[Serializable]
	public class Gesture
	{
		public float PosePrecision;

		public string CompatibleGestures;

		public Vector3 RootTransformPosition;
		public PoseDirection RootTransformDirection;
		public Vector3 PreciseRootTransformDirection;
		public PoseDirection RootTransformOrientation;
		public Vector3 PreciseRootTransformOrientation;

		public Dictionary<string, float> TrackedBoneDistances;
		public Dictionary<string, Vector3> TrackedBonePositions;
	}
}
