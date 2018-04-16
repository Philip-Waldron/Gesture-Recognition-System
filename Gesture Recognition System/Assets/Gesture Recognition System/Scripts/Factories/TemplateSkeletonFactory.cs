using System;
using Gesture_Recognition_System.Scripts.Enums;
using Gesture_Recognition_System.Scripts.Skeletons;

namespace Gesture_Recognition_System.Scripts.Factories
{
	public class TemplateSkeletonFactory
	{
		public static TemplateSkeleton NewTemplate(GestureObjectType type)
		{
			switch (type)
			{
				case GestureObjectType.Custom:
					return new CustomTemplateSkeleton();
				case GestureObjectType.Hand:
					return new HandTemplateSkeleton();
				default:
					throw new NotSupportedException();
			}
		}
	}
}
