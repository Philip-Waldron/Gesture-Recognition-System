using Gesture_Recognition_System.Scripts.Enums;
using Gesture_Recognition_System.Scripts.Factories;
using Gesture_Recognition_System.Scripts.Skeletons;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gesture_Recognition_System.Scripts.Gestures
{
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	public class GestureObject : SerializedMonoBehaviour
	{
		[SerializeField] [OnValueChanged("UpdateReferences")]
		private GestureObjectType _objectType = GestureObjectType.Custom;
		 
		[SerializeField]
		public Transform TrackThisInRelationTo;
		
		[SerializeField] [HideReferenceObjectPicker]
		public Skeleton Skeleton;
		
		private void UpdateReferences()
		{
			CreateTemplate();
		}

		void Reset()
		{
			TrackThisInRelationTo = Camera.main.transform;
			CreateTemplate();
		}

		void OnDisable()
		{
			if (GestureManager.Instance != null)
			{
				GestureManager.Instance.RemoveGestureObject(this);
			}
		}

		void OnEnable()
		{
			GestureManager.Instance.AddGestureObject(this);
		}

		private void CreateTemplate()
		{
			TemplateSkeleton template = TemplateSkeletonFactory.NewTemplate(_objectType);
			Skeleton = template.GenerateTemplate(transform);
		}
	}
}
