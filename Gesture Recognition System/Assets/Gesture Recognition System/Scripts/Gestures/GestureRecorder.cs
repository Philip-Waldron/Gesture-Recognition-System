using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
#endif

namespace Gesture_Recognition_System.Scripts.Gestures
{
    public class GestureRecorder : SerializedMonoBehaviour
    {
        [SerializeField]
        [ListDrawerSettings(CustomAddFunction = "CustomAddObjectsButton", DraggableItems = false, Expanded = true), HideReferenceObjectPicker]
        private List<RecordObjectStructure> _gestureObjectsToRecord = new List<RecordObjectStructure>();

        void Update()
        {
            if (Application.isPlaying)
            {
                foreach (var obj in _gestureObjectsToRecord)
                {
                    obj.UpdateTrackedBones();
                    obj.UpdateRootTransform();
                }
            }
        }

#if UNITY_EDITOR
        private void CustomAddObjectsButton()
        {
            // Gets all GestureObjeects not in TrackedObjects list and created a popup selector with support for multi-selection
            List<GestureObject> availableGestureObjects = GestureManager.Instance.GetGestureObjects()
                .Where(x => !_gestureObjectsToRecord.Select(y => y.GestureObject.Skeleton).Contains(x.Skeleton))
                .ToList();
            
            var selector = new GenericSelector<GestureObject>(null, true, x => x.Skeleton.StructureName, availableGestureObjects);
            selector.ShowInPopup(Event.current.mousePosition + Vector2.left * 135, 135);
            selector.SelectionTree.Config.DrawSearchToolbar = false;
            selector.SelectionConfirmed += sel =>
            {
                var gestureObjects = sel as IList<GestureObject> ?? sel.ToList();
                foreach (var gestureObject in gestureObjects)
                {
                    _gestureObjectsToRecord.Add(new RecordObjectStructure(gestureObject));
                }
            };
        }
#endif
    }
}
