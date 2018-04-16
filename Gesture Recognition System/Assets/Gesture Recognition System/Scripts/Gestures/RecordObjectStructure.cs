using System;
using System.Collections.Generic;
using Gesture_Recognition_System.Scripts.Enums;
using Gesture_Recognition_System.Scripts.ScriptableObjects;
using Gesture_Recognition_System.Scripts.Skeletons;
using Gesture_Recognition_System.Scripts.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace Gesture_Recognition_System.Scripts.Gestures
{
    public class RecordObjectStructure
    {
        [SerializeField] [ReadOnly]
        public GestureObject GestureObject;

        [SerializeField] [ReadOnly]
        public Skeleton Skeleton;
        
        [SerializeField] [Title("$_gestureName", bold: true)]
        private string _gestureName = "Default Gesture";
        
        [SerializeField] [Range(0, 100)]
        private float _posePrecision = 100;
        
        // RootTransform
        [SerializeField] [BoxGroup("Root Transform"), LabelText("Record Position")]
        private bool _recordRootTransformPosition = true;
        [SerializeField] [BoxGroup("Root Transform"), LabelText("Position"), EnableIf("_recordRootTransformPosition")]
        private Vector3 _rootTransformPosition;
        
        [SerializeField] [BoxGroup("Root Transform"), LabelText("Record Direction")]
        private bool _recordRootTransformDirection = true;
        [SerializeField] [BoxGroup("Root Transform"), LabelText("Use Precise Direction")] [EnableIf("_recordRootTransformDirection")]
        private bool _usePreciseRootTransformDirection;
        [SerializeField] [BoxGroup("Root Transform"), LabelText("Direction"), HideIf("_usePreciseRootTransformDirection"), EnableIf("_recordRootTransformDirection")]
        private PoseDirection _rootTransformDirection;
        [SerializeField] [BoxGroup("Root Transform"), LabelText("Direction"), ShowIf("_usePreciseRootTransformDirection"), EnableIf("_recordRootTransformDirection")]
        private Vector3 _preciseRootTransformDirection;
        
        [SerializeField] [BoxGroup("Root Transform"), LabelText("Record Orientation")]
        private bool _recordRootTransformOrientation = true;
        [SerializeField] [BoxGroup("Root Transform"), LabelText("Use Precise Orientation")] [EnableIf("_recordRootTransformOrientation")]
        private bool _usePreciseRootTransformOrientation;
        [SerializeField] [BoxGroup("Root Transform"), LabelText("Orientation"), HideIf("_usePreciseRootTransformOrientation"), EnableIf("_recordRootTransformOrientation")]
        private PoseDirection _rootTransformOrientation;
        [SerializeField]  [BoxGroup("Root Transform"), LabelText("Orientation"), ShowIf("_usePreciseRootTransformOrientation"), EnableIf("_recordRootTransformOrientation")]
        private Vector3 _preciseRootTransformOrientation;
        
        // TrackedBones
        [SerializeField] [TableList(IsReadOnly = true)]
        private List<RecordTrackedBones> _trackedBones = new List<RecordTrackedBones>();

        // Constructor
        public RecordObjectStructure(GestureObject gestureObject)
        {
            this.GestureObject = gestureObject;
            this.Skeleton = GestureObject.Skeleton;

            foreach (var bone in gestureObject.Skeleton.TrackedBones)
            {
                _trackedBones.Add(new RecordTrackedBones(bone.Key));
            }
        }
        
        // Record button
        [Button(ButtonSizes.Large)]
        private void RecordGesture()
        {
            if (!Application.isPlaying)
            {
                if (!EditorUtility.DisplayDialog("Record Gesture", "Are you sure you want to record a gesture outside of playmode?", "Yes", "Cancel"))
                {
                    return;
                }
            }

            ScriptableGesture scriptableGesture = ScriptableObjectUtility.CreateAsset<ScriptableGesture>("Assets/Resources/Gestures", _gestureName);
            Gesture gesture = new Gesture();
            
            RecordGesture(gesture);

            scriptableGesture.Object = gesture;
            
            Debug.Log("Recoreded a new gesture called: " + _gestureName);
        }
        
        private void RecordGesture(Gesture gesture)
        {
            gesture.PosePrecision = _posePrecision;

            gesture.CompatibleGestures = GestureObject.Skeleton.SkeletonGroup;

            gesture.RootTransformPosition = _recordRootTransformPosition ? _rootTransformPosition : Vector3.zero;
            gesture.RootTransformDirection = _recordRootTransformDirection ? _rootTransformDirection : default(PoseDirection);
            gesture.PreciseRootTransformDirection = _usePreciseRootTransformDirection ? _preciseRootTransformDirection : Vector3.zero;
            gesture.RootTransformOrientation = _recordRootTransformOrientation ? _rootTransformOrientation : default(PoseDirection);
            gesture.PreciseRootTransformOrientation = _usePreciseRootTransformOrientation ? _preciseRootTransformOrientation : Vector3.zero;

            Dictionary<string, Vector3> bonesPositions = new Dictionary<string, Vector3>();
            Dictionary<string, float> bonesDistances = new Dictionary<string, float>();
            foreach (var bone in _trackedBones)
            {
                if (bone.RecordBone)
                {
                    if (bone.UseDistance)
                    {
                        bonesDistances.Add(bone.Name, bone.Distance);
                    }
                    else
                    {
                        bonesPositions.Add(bone.Name, bone.Position);
                    }
                }
                else
                {
                    bonesPositions.Add(bone.Name, Vector3.zero);
                    bonesDistances.Add(bone.Name, 0);
                }
            }

            gesture.TrackedBonePositions = bonesPositions;
            gesture.TrackedBoneDistances = bonesDistances;
        }

        public void UpdateTrackedBones()
        {
            foreach (var bone in _trackedBones)
            {
                if (bone.UseDistance)
                {
                    bone.Distance = bone.RecordBone
                        ? GestureObject.Skeleton.GetRelativeBoneDistance(bone.Name)
                        : 0;
                }
                else
                {
                    bone.Position = bone.RecordBone
                        ? GestureObject.Skeleton.GetRelativeBonePosition(bone.Name)
                        : Vector3.zero;
                }
            }
        }

        public void UpdateRootTransform()
        {
            if (_usePreciseRootTransformDirection)
            {
                _preciseRootTransformDirection = _recordRootTransformDirection
                    ? GestureObject.Skeleton.GetRootTransformVector3Direction(GestureObject.TrackThisInRelationTo)
                    : Vector3.zero;
            }
            else
            {
                _rootTransformDirection = _recordRootTransformDirection
                    ? GestureObject.Skeleton.GetRootTransformPoseDirection(GestureObject.TrackThisInRelationTo)
                    : PoseDirection.Undefined;
            }
            if (_usePreciseRootTransformOrientation)
            {
                _preciseRootTransformOrientation = _recordRootTransformOrientation
                    ? GestureObject.Skeleton.GetRootTransformVector3Orientation(GestureObject.TrackThisInRelationTo)
                    : Vector3.zero;
            }
            else
            {
                _rootTransformOrientation = _recordRootTransformOrientation
                    ? GestureObject.Skeleton.GetRootTransformPoseOrientation(GestureObject.TrackThisInRelationTo)
                    : PoseDirection.Undefined;
            }

            _rootTransformPosition = _recordRootTransformPosition
                ? GestureObject.Skeleton.GetRootTransformRelativePosition(GestureObject.TrackThisInRelationTo)
                : Vector3.zero;
        }

        // Class to represent TrackedBone item in list
        [Serializable]
        private class RecordTrackedBones
        {
            [SerializeField]
            [DisplayAsString()] [HorizontalGroup("")]
            [LabelWidth(40)]
            [HideLabel]
            private string _record = "Record ";
            
            [SerializeField] [HideInInspector]
            public string Name;
            
            [SerializeField]
            [HorizontalGroup("")]
            [HideLabel]
            public bool RecordBone = true;
            
            [SerializeField]
            [HorizontalGroup("")]
            [LabelWidth(80)]
            public bool UseDistance = true;

            [SerializeField]
            [HorizontalGroup("")]
            [LabelText("Position:")]
            [EnableIf("RecordBone")]
            [HideIf("UseDistance")]
            [LabelWidth(60)]
            public Vector3 Position;
            
            [SerializeField]
            [HorizontalGroup("")]
            [LabelText("Distance:")]
            [EnableIf("RecordBone")]
            [ShowIf("UseDistance")]
            public float Distance;

            public RecordTrackedBones(string boneName)
            {
                _record += boneName;
                Name = boneName;
            }
        }
    }
}
#endif
