using Sirenix.OdinInspector;

namespace Gesture_Recognition_System.Scripts.ScriptableObjects
{
    public class ScriptableGesture : SerializedScriptableObject
    {
        public object Object;

        public T GetGesture<T>()
        {
            return (T) Object;
        }
    }
}
