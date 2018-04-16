using UnityEngine;

namespace Gesture_Recognition_System.Scripts.Skeletons
{
    public abstract class TemplateSkeleton
    {
        public abstract Skeleton GenerateTemplate(Transform rootTransform);
    }
}
