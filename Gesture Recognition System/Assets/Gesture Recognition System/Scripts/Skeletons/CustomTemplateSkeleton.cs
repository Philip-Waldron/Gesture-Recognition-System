using Gesture_Recognition_System.Scripts.Builders;
using UnityEngine;

namespace Gesture_Recognition_System.Scripts.Skeletons
{
	public class CustomTemplateSkeleton : TemplateSkeleton
	{
		public override Skeleton GenerateTemplate(Transform rootTransform)
		{
			Skeleton handSkeleton = new FluentSkeletonBuilder().CreateSkeleton(rootTransform.name).WithRootBone(rootTransform);
			return handSkeleton;
		}
	}
}
