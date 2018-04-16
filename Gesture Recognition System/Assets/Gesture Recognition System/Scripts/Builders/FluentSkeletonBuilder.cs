using System.Collections.Generic;
using Gesture_Recognition_System.Scripts.Skeletons;
using UnityEngine;

namespace Gesture_Recognition_System.Scripts.Builders
{
	public class FluentSkeletonBuilder
	{
		private string _structureName = "";
		private Transform _rootBone;
		private Dictionary<string, Transform> _trackedBones = new Dictionary<string, Transform>();
		private string _skeletonGroup = "Ungrouped";

		public FluentSkeletonBuilder CreateSkeleton(string structureName)
		{
			_structureName = structureName;
			return this;
		}
		
		public FluentSkeletonBuilder WithRootBone(Transform rootBone)
		{
			_rootBone = rootBone;
			return this;
		}
		
		public FluentSkeletonBuilder WithTrackedBones(Dictionary<string, Transform> trackedBones)
		{
			_trackedBones = trackedBones;
			return this;
		}
		
		public FluentSkeletonBuilder InSkeletonGroup(string skeletonGroup)
		{
			_skeletonGroup = skeletonGroup;
			return this;
		}

		public static implicit operator Skeleton(FluentSkeletonBuilder fsb)
		{
			return new Skeleton(
				fsb._structureName,
				fsb._rootBone,
				fsb._trackedBones,
				fsb._skeletonGroup);
		}
	}
}
