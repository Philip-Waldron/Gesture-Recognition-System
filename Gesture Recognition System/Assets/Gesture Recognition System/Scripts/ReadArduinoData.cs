using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using UnityEngine;

namespace Gesture_Recognition_System.Scripts
{
    public class ReadArduinoData : MonoBehaviour
    {
        SerialPort sp = new SerialPort("COM3", 38400);
        // posx, posy, posz, pitch, yaw, roll
        float[][] calibration = new float[6][];
        // Thumb, index, middle, ring, pinky, palm
        [SerializeField]
        Transform[] hand = new Transform[6];
        [SerializeField]
        GameObject defaultPosition;

        void Start()
        {
            sp.Open();
            sp.ReadTimeout = 1;
        }

        void Update()
        {
            if (sp.IsOpen)
            {
                StartCoroutine
                (AsynchronousReadFromSerialPort(() => Debug.LogError("Error!"), 1f));
            }
        }

        [Button()]
        private void CalibrateSensors()
        {
            transform.position = defaultPosition.transform.position;
            for (int count = 0; count < calibration.Length; count++)
            {
                float[] offset =
                {
                    hand[count].position.x,
                    hand[count].position.y,
                    hand[count].position.z,
                    hand[count].rotation.x,
                    hand[count].rotation.y,
                    hand[count].rotation.z,
                };
                calibration[count] = offset;
            }
        }

        public IEnumerator AsynchronousReadFromSerialPort(Action fail = null, float timeout = float.PositiveInfinity)
        {
            DateTime initialTime = DateTime.Now;
            DateTime nowTime;
            TimeSpan diff = default(TimeSpan);

            List<string> data = null;
            string[] lines;

            while (diff.Seconds < timeout)
            {
                try
                {
                    string unsplitLine = "";
                    sp.ReadTo(unsplitLine);
                    lines = unsplitLine.Split(
                        new[] { Environment.NewLine },
                        StringSplitOptions.None
                        );
                    foreach (string line in lines)
                    {
                        data = sp.ReadLine().Split(',').ToList();
                        if (data != null && data.Count == 3)
                        {
                            switch (data[1])
                            {
                                case "accx":
                                    hand[int.Parse(data[0])].position += new Vector3(int.Parse(data[2]) - calibration[int.Parse(data[0])][0], 0, 0);
                                    break;
                                case "accy":
                                    hand[int.Parse(data[0])].position += new Vector3(0, int.Parse(data[2]) - calibration[int.Parse(data[0])][1], 0);
                                    break;
                                case "accz":
                                    hand[int.Parse(data[0])].position += new Vector3(0, 0, int.Parse(data[2]) - calibration[int.Parse(data[0])][2]);
                                    break;
                                case "pitch":
                                    hand[int.Parse(data[0])].eulerAngles = new Vector3(int.Parse(data[2]) - calibration[int.Parse(data[0])][3], 0, 0);
                                    break;
                                case "yaw":
                                    hand[int.Parse(data[0])].eulerAngles = new Vector3(0, int.Parse(data[2]) - calibration[int.Parse(data[0])][4], 0);
                                    break;
                                case "roll":
                                    hand[int.Parse(data[0])].eulerAngles = new Vector3(0, 0, int.Parse(data[2]) - calibration[int.Parse(data[0])][5]);
                                    break;
                            }
                        }
                    }
                }
                catch (TimeoutException)
                {
                    data = null;
                }

                if (data != null)
                {
                    yield return null;
                }
                else
                {
                    yield return new WaitForSeconds(0.1f);
                }

                nowTime = DateTime.Now;
                diff = nowTime - initialTime;

            }

            if (fail != null)
            {
                fail();
            }
            yield return null;
        }
    }
}
