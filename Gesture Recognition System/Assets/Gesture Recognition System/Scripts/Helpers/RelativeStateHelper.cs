using Gesture_Recognition_System.Scripts.Enums;
using UnityEngine;

namespace Gesture_Recognition_System.Scripts.Helpers
{
	public class RelativeStateHelper
	{
		public static PoseDirection CalculateRelativeDirection(Transform origin, Vector3 direction)
		{
			Vector3[] directions = {
				origin.forward, origin.forward * -1,
				origin.up, origin.up * -1,
				origin.right, origin.right * -1};

			PoseDirection[] poses = {
				PoseDirection.Forward,
				PoseDirection.Backward,
				PoseDirection.Up,
				PoseDirection.Down,
				PoseDirection.Right,
				PoseDirection.Left };
		
			float score = float.MaxValue;
			PoseDirection closestPose = PoseDirection.Undefined;
		
			for (int i = 0; i < 6; i++)
			{
				float tempScore = Vector3.Angle(directions[i], direction);
				if (tempScore < score)
				{
					score = tempScore;
					closestPose = poses[i];
				}
			}
		
			return closestPose;
		}

		public static Vector3 CalculateRelativePosition(Transform origin, Transform subject)
		{
			Vector3 distance = subject.position - origin.position;
			Vector3 relativePosition = Vector3.zero;
			relativePosition.x = Vector3.Dot(distance, origin.right.normalized);
			relativePosition.y = Vector3.Dot(distance, origin.up.normalized);
			relativePosition.z = Vector3.Dot(distance, origin.forward.normalized);

			return relativePosition;
		}
		
		public static float CalculateRelativeDistance(Transform origin, Transform subject)
		{
			return Vector3.Distance(origin.position, subject.position);
		}
	}
}
