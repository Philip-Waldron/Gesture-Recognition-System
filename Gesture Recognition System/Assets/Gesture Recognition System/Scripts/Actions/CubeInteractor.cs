using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gesture_Recognition_System.Scripts.Actions
{
	public class CubeInteractor : MonoBehaviour
	{
		public GameObject SpawnObj;
		public float XMax = 10f;
		public float XMin = -10f;
		public float ZMax = -10f;
		public float ZMin = 10f;

		private bool _charge = false;
		
		private List<GameObject> _cubes = new List<GameObject>();

		public void SpawnOBject()
		{
			_cubes.Add(Instantiate(SpawnObj, 
				new Vector3(Random.Range(XMin, XMax),10,Random.Range(ZMax, ZMin)), 
				Quaternion.Euler(new Vector3(Random.Range(-180,180),Random.Range(-180,180),Random.Range(-180,180)))));
		}

		public void AddForceToCubes(float power)
		{
			if (_charge == true)
			{
				print("Apply Force");
				foreach (var cube in _cubes)
				{
					cube.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * power);
				}
				_charge = false;
			}
		}

		public void ChargeForce()
		{
            print("Charging");
            _charge = true;
		}
		
		//private IEnumerator Timer(float waitTime)
		//{
		//	yield return new WaitForSeconds(waitTime);
		//	_charge = false;
		//}
	}
}
