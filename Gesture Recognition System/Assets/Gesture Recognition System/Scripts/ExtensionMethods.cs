using Gesture_Recognition_System.Scripts.Enums;
using Gesture_Recognition_System.Scripts.Helpers;
using Gesture_Recognition_System.Scripts.Skeletons;
using UnityEngine;

namespace Gesture_Recognition_System.Scripts
{
    public static class ExtensionMethods
    {
        public static Transform DeepestImmediateChild(this Transform transform)
        {
            while (transform.childCount != 0)
            {
                transform = transform.GetChild(0);
            }
            return transform;
        }
        
        public static PoseDirection GetRootTransformPoseDirection(this Skeleton skeleton, Transform relativeTo)
        {
            return skeleton.RootBone.gameObject.activeInHierarchy == false
                ? PoseDirection.Undefined
                : RelativeStateHelper.CalculateRelativeDirection(relativeTo, skeleton.RootBone.up * -1);
        }

        public static Vector3 GetRootTransformVector3Direction(this Skeleton skeleton, Transform relativeTo)
        {
            return skeleton.RootBone.gameObject.activeInHierarchy == false
                ? Vector3.zero
                : (skeleton.RootBone.up * -1) - relativeTo.forward;
        }

        public static PoseDirection GetRootTransformPoseOrientation(this Skeleton skeleton, Transform relativeTo)
        {
            return skeleton.RootBone.gameObject.activeInHierarchy == false
                ? PoseDirection.Undefined
                : RelativeStateHelper.CalculateRelativeDirection(relativeTo, skeleton.RootBone.forward * -1);
        }

        public static Vector3 GetRootTransformVector3Orientation(this Skeleton skeleton, Transform relativeTo)
        {
            return skeleton.RootBone.gameObject.activeInHierarchy == false
                ? Vector3.zero
                : (skeleton.RootBone.forward * -1) - relativeTo.forward;
        }
        
        public static Vector3 GetRootTransformRelativePosition(this Skeleton skeleton, Transform relativeTo)
        {
            return skeleton.RootBone.gameObject.activeInHierarchy == false
                ? Vector3.zero
                : RelativeStateHelper.CalculateRelativePosition(relativeTo, skeleton.RootBone);
        }
        
        public static Vector3 GetRelativeBonePosition(this Skeleton skeleton, string bone)
        {
            Transform value = skeleton.TrackedBones[bone];
            
            return RelativeStateHelper.CalculateRelativePosition(skeleton.RootBone, value);
        }        
        
        public static float GetRelativeBoneDistance(this Skeleton skeleton, string bone)
        {
            Transform value = skeleton.TrackedBones[bone];
            
            return RelativeStateHelper.CalculateRelativeDistance(skeleton.RootBone, value);
        }
    }
}
