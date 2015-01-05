using System;
using UnityEngine;

namespace HandMenu {

	/*================================================================================================*/
	public class FingerDisplay : MonoBehaviour {

		public Func<FingerData> GetCurrentData;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			var bar = GameObject.CreatePrimitive(PrimitiveType.Cube);
			bar.transform.parent = gameObject.transform;
			bar.transform.localScale = new Vector3(1, 1, 10)*0.002f;
			bar.transform.localPosition = new Vector3(0, 0, bar.transform.localScale.z*2);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			FingerData data = GetCurrentData();

			if ( data == null ) {
				return;
			}

			gameObject.transform.localPosition = data.Position;
			gameObject.transform.localRotation = 
				Quaternion.FromToRotation(Vector3.back, data.Direction);
		}

	}

}
