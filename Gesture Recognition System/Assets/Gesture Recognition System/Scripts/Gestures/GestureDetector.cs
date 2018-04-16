using System.Collections.Generic;
using System.Linq;
using Gesture_Recognition_System.Scripts.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
#endif

namespace Gesture_Recognition_System.Scripts.Gestures
{
	public class GestureDetector : SerializedMonoBehaviour
	{
		[SerializeField]
		[ListDrawerSettings(CustomAddFunction = "CustomAddObjectsButton", DraggableItems = false, Expanded = true), HideReferenceObjectPicker]
		private List<DetectObjectStructure> _gestureObjectsToTrack = new List<DetectObjectStructure>();

		void Update()
		{
			if (Application.isPlaying)
			{
				foreach (var obj in _gestureObjectsToTrack)
				{
					for (int c = 0; c < obj.CodeBlocks.Count; c++)
					{
						bool detected = obj.DetectGesture(obj.CodeBlocks[c].Gesture);
						
						if (obj.CodeBlocks[c].DisableConjunction == true) // Last value
						{
							if (detected == true)
							{
								obj.ConstantEvent.Invoke();
								if (!obj.Activated)
								{
									obj.Activated = true;
                                    obj.SingleEvent.Invoke();
                                }
							}
							else
							{
								obj.Activated = false;
							}
						}
						else if (obj.CodeBlocks[c].Conjunction == Conjunction.And)
						{
							if (detected == false)
							{
                                obj.Activated = false;
                                break;
							}
						}
						else if (obj.CodeBlocks[c].Conjunction == Conjunction.Or)
						{
							if (detected == true)
							{
								c++;
								if (c >= obj.CodeBlocks.Count)
								{
									obj.ConstantEvent.Invoke();
									if (!obj.Activated)
									{
										obj.SingleEvent.Invoke();
										obj.Activated = true;
									}
								}
							}
						}
						
						//Not implemented
						else if (obj.CodeBlocks[c].Conjunction == Conjunction.Then)
						{
							if (detected == false)
							{
                                obj.Activated = false;
                                break;
							}
						}
					}
				}
			}
		}
		
#if UNITY_EDITOR
		private void CustomAddObjectsButton()
		{
			// Gets all GestureObjeects not in TrackedObjects list and created a popup selector with support for multi-selection
			List<GestureObject> availableGestureObjects = GestureManager.Instance.GetGestureObjects();
            
			var selector = new GenericSelector<GestureObject>(null, true, x => x.Skeleton.StructureName, availableGestureObjects);
			selector.ShowInPopup(Event.current.mousePosition + Vector2.left * 135, 135);
			selector.SelectionTree.Config.DrawSearchToolbar = false;
			selector.SelectionConfirmed += sel =>
			{
				var gestureObjects = sel as IList<GestureObject> ?? sel.ToList();
				foreach (var gestureObject in gestureObjects)
				{
					_gestureObjectsToTrack.Add(new DetectObjectStructure(gestureObject));
				}
			};
		}
#endif
	}
}
