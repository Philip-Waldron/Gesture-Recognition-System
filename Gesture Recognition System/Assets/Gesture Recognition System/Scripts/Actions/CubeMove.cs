using UnityEngine;

namespace Gesture_Recognition_System.Scripts
{
    public class CubeMove : MonoBehaviour {
        public void MoveForward()
        {
            transform.position += new Vector3(0.01f, 0, 0);
        }
    }
}
