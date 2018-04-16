using System.Collections.Generic;
using System.Linq;
using Gesture_Recognition_System.Scripts.Builders;
using UnityEngine;

namespace Gesture_Recognition_System.Scripts.Skeletons
{
    public class HandTemplateSkeleton : TemplateSkeleton
    {
        private string _name = "";
        private Dictionary<string, Transform> _bones;
        private Transform _palm;
        private Transform _rootTransform;
        
        public override Skeleton GenerateTemplate(Transform rootTransform)
        {
            _rootTransform = rootTransform;
            GenerateName();
            CreateHandDictionary();
            GenerateBones();
            
            Skeleton handSkeleton = new FluentSkeletonBuilder().CreateSkeleton(_name).WithRootBone(_palm).WithTrackedBones(_bones).InSkeletonGroup("Hands");
            return handSkeleton;
        }

        private void GenerateName()
        {
            _name = _rootTransform.name;
            if (_rootTransform.name.ToLower().Contains("left"))
            {
                _name = "Left Hand";
            }
            else if (_rootTransform.name.ToLower().Contains("right"))
            {
                _name = "Right Hand";
            }
            else if (_rootTransform.name.ToLower().Contains("hand"))
            {
                _name = "Hand";
            }
        }
        
        private void CreateHandDictionary()
        {
            _bones = new Dictionary<string, Transform>
            {
                {"Pinky", null},
                {"Ring", null},
                {"Middle", null},
                {"Index", null},
                {"Thumb", null}
            };
        }

        private void GenerateBones()
        {
            Transform[] children = _rootTransform.GetComponentsInChildren<Transform>();
            foreach (Transform child in children)
            {
                if (_palm == null && child.name.ToLower().Contains("palm"))
                {
                    _palm = child;
                }
                
                if (_bones["Pinky"] == null && child.name.ToLower().Contains("pinky"))
                {
                    _bones["Pinky"] = child.DeepestImmediateChild();
                }
                
                if (_bones["Ring"] == null && child.name.ToLower().Contains("ring"))
                {
                    _bones["Ring"] = child.DeepestImmediateChild();
                }
                
                if (_bones["Middle"] == null && child.name.ToLower().Contains("middle"))
                {
                    _bones["Middle"] = child.DeepestImmediateChild();
                }
                
                if (_bones["Index"] == null && child.name.ToLower().Contains("index"))
                {
                    _bones["Index"] = child.DeepestImmediateChild();
                }
                
                if (_bones["Thumb"] == null && child.name.ToLower().Contains("thumb"))
                {
                    _bones["Thumb"] = child.DeepestImmediateChild();
                }
                
                if (_bones.Values.ToList().All(x => x != null) && _palm != null)
                {
                    break;
                }
            }

            if (_palm == null)
            {
                _palm = _rootTransform;
            }
        }
    }
}
