using System;
using Leap;
using UnityEngine;

namespace HandMenu {

	/*================================================================================================*/
	public class FingerDisplay : MonoBehaviour {

		public Func<Finger> GetCurrentFinger;


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
			Finger finger = GetCurrentFinger();

			if ( finger == null ) {
				return;
			}

			Vector3 tip = finger.TipPosition.ToUnityScaled();
			Vector3 dir = finger.Bone(Bone.BoneType.TYPE_DISTAL).Direction.ToUnity();

			gameObject.transform.localPosition = tip;
			gameObject.transform.localRotation = Quaternion.FromToRotation(Vector3.back, dir);
		}

	}

}
