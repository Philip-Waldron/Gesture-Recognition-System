using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gesture_Recognition_System.Scripts.Skeletons
{
	public class Skeleton
	{
		[SerializeField] public string StructureName { get; private set; }
		[SerializeField] public Transform RootBone { get; private set; }
		
		[SerializeField] [DictionaryDrawerSettings(KeyLabel = "Bone", ValueLabel = "Transform")]
		public Dictionary<string, Transform> TrackedBones { get; private set; }
		
		// Skeleton group is used for defining cross compatible gestures, validation TO DO
		[SerializeField] public string SkeletonGroup { get; private set; }

		public Skeleton(string structureName, Transform rootBone, Dictionary<string, Transform> trackedBones, string skeletonGroup)
		{
			StructureName = structureName;
			RootBone = rootBone;
			TrackedBones = trackedBones;
			SkeletonGroup = skeletonGroup;
		}
	}
}
