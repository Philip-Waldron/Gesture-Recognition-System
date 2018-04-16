using System;
using System.Collections.Generic;
using System.Linq;
using Gesture_Recognition_System.Scripts.Enums;
using Gesture_Recognition_System.Scripts.ScriptableObjects;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
#endif

namespace Gesture_Recognition_System.Scripts.Gestures
{
	public class DetectObjectStructure
	{
		[SerializeField]
		[HideInInspector]
		private string _name;
		
		[SerializeField] 
		[FoldoutGroup("$_name")]
		[HideInInspector]
		public GestureObject GestureObject;

		[SerializeField]
		[FoldoutGroup("$_name")] 
		[LabelText("If")]
		[OnValueChanged("UpdateReferences")]
		[ListDrawerSettings(CustomAddFunction = "CustomAddObjectsButton"), HideReferenceObjectPicker]
		public List<CodeBlock> CodeBlocks = new List<CodeBlock>();
		
		[SerializeField]
		[FoldoutGroup("$_name")]
		[HideReferenceObjectPicker]
		public UnityEvent ConstantEvent = new UnityEvent();
		
		[SerializeField]
		[FoldoutGroup("$_name")]
		[HideReferenceObjectPicker]
		public UnityEvent SingleEvent = new UnityEvent();

		[HideInInspector]
		public bool Activated = false;
		
		public DetectObjectStructure(GestureObject gestureObject)
		{
			this.GestureObject = gestureObject;
			this._name = gestureObject.Skeleton.StructureName;
		}
		
		public List<ScriptableGesture> GetUsableGestures()
		{
			return Resources.LoadAll<ScriptableGesture>("Gestures").ToList()
				.Where(x => x.Object.GetType() == typeof(Gesture))
				.Where(x => x.GetGesture<Gesture>().CompatibleGestures == GestureObject.Skeleton.SkeletonGroup)
				.ToList();
		}

		void UpdateReferences()
		{
			if (CodeBlocks.Count > 0)
			{
				CodeBlocks[CodeBlocks.Count - 1].DisableConjunction = true;
			}
			if (CodeBlocks.Count > 1)
			{
				CodeBlocks[CodeBlocks.Count - 2].DisableConjunction = false;
			}
		}

		public bool DetectGesture(Gesture gesture)
		{
			float precision;
            if (gesture.PosePrecision == 100)
            {
                precision = 0.2f;
            }
            else
            {
                precision = 0.2f * ((100 - gesture.PosePrecision) / 100);
            }
			
			if (gesture.RootTransformPosition != Vector3.zero)
			{
				Vector3 position = GestureObject.Skeleton.GetRootTransformRelativePosition(GestureObject.TrackThisInRelationTo);
                
                if (Vector3.Distance((Vector3)gesture.RootTransformPosition, position) > precision)
                {
                    return false;
                }
			}

			if (gesture.RootTransformDirection != PoseDirection.Undefined)
			{
                if (gesture.RootTransformDirection != GestureObject.Skeleton.GetRootTransformPoseDirection(GestureObject.TrackThisInRelationTo))
                {
                    return false;
                }
			}

			if (gesture.PreciseRootTransformDirection != Vector3.zero)
			{
				Vector3 position = GestureObject.Skeleton.GetRootTransformVector3Direction(GestureObject.TrackThisInRelationTo);
                
				if (Vector3.Distance((Vector3)gesture.PreciseRootTransformDirection, position) > precision)
				{
					return false;
				}
			}

			if (gesture.RootTransformOrientation != PoseDirection.Undefined)
			{
				if (gesture.RootTransformOrientation != GestureObject.Skeleton.GetRootTransformPoseOrientation(GestureObject.TrackThisInRelationTo))
				{
					return false;
				}
			}

			if (gesture.PreciseRootTransformOrientation != Vector3.zero)
			{
				Vector3 position = GestureObject.Skeleton.GetRootTransformVector3Orientation(GestureObject.TrackThisInRelationTo);
                
				if (Vector3.Distance((Vector3)gesture.PreciseRootTransformOrientation, position) > precision)
				{
					return false;
				}
			}

			foreach (var bone in gesture.TrackedBonePositions)
			{
				if (bone.Value != Vector3.zero)
				{
					var position = GestureObject.Skeleton.GetRelativeBonePosition(bone.Key);

					if (Vector3.Distance((Vector3)gesture.TrackedBonePositions[bone.Key], position) > precision)
					{
						return false;
					}
				}
			}
			
			foreach (var bone in gesture.TrackedBoneDistances)
			{
				if (bone.Value != 0)
				{
					var distance = GestureObject.Skeleton.GetRelativeBoneDistance(bone.Key);

					if ((gesture.TrackedBoneDistances[bone.Key] - distance) > 0.003)
					{
						return false;
					}
				}
			}

			return true;
		}
		
#if UNITY_EDITOR
		private void CustomAddObjectsButton()
		{
			var usableGestures = GetUsableGestures()
				.Where(x => !CodeBlocks.Select(y => y.Gesture)
					.Contains(x.GetGesture<Gesture>()));
			
			var selector = new GenericSelector<ScriptableGesture>(null, true, x => x.name, usableGestures);
			selector.ShowInPopup(Event.current.mousePosition + Vector2.left * 135, 135);
			selector.SelectionTree.Config.DrawSearchToolbar = false;
			selector.SelectionConfirmed += sel =>
			{
				var gestures = sel as IList<ScriptableGesture> ?? sel.ToList();
				foreach (var gesture in gestures)
				{
					CodeBlocks.Add(new CodeBlock(gesture.GetGesture<Gesture>(), gesture.name));
				}

				CodeBlocks[CodeBlocks.Count - 1].DisableConjunction = true;
				if (CodeBlocks.Count > 1)
				{
					CodeBlocks[CodeBlocks.Count - 2].DisableConjunction = false;
				}
			};
		}
#endif
		
		[Serializable]
		public class CodeBlock
		{
			[SerializeField]
			[HideInInspector]
			private string _gestureName;
			
			[SerializeField]
			[HideInInspector]
			public bool DisableConjunction;
			
			[SerializeField]
			[HideInInspector]
			public Gesture Gesture;
			
			[SerializeField]
			[TitleGroup("$_gestureName")]
			[ShowIf("ConjunctionIsThen")] [HideIf("DisableConjunction")]
			public float TimeToCompleteNextGesture;
			
			[SerializeField] [HideLabel] [HorizontalGroup(MaxWidth = 70)] [HideIf("DisableConjunction")]
			public Conjunction Conjunction;
			
			private bool ConjunctionIsThen { get { return (Conjunction == Conjunction.Then); } }

			public CodeBlock(Gesture gesture, string gestureName)
			{
				Gesture = gesture;
				_gestureName = gestureName;
			}
		}
	}
}
