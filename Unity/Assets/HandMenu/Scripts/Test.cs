using System.Collections.Generic;
using System.Linq;
using Leap;
using UnityEngine;

namespace HandMenu {

	/*================================================================================================*/
	public class Test : MonoBehaviour {

		private GameObject vHandControlObj;
		private Controller vControl;
		private IList<GameObject> vFingerObjList;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vHandControlObj = GameObject.Find("HandController");
			vFingerObjList = new List<GameObject>();

			for ( int i = 0 ; i < 5 ; ++i ) {
				var fingerObj = new GameObject("Finger"+i);
				fingerObj.transform.parent = vHandControlObj.transform;
				vFingerObjList.Add(fingerObj);

				var bar = GameObject.CreatePrimitive(PrimitiveType.Cube);
				bar.transform.parent = fingerObj.transform;
				bar.transform.localScale = new Vector3(1, 1, 10)*0.002f;
				bar.transform.localPosition = new Vector3(0, 0, bar.transform.localScale.z*2);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			vControl = vHandControlObj.GetComponent<HandController>().GetLeapController();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			OVRManager.capiHmd.DismissHSWDisplay();

			////

			Frame frame = vControl.Frame();
			Hand hand = frame.Hands.FirstOrDefault();

			foreach ( GameObject fingerObj in vFingerObjList ) {
				fingerObj.SetActive(hand != null);
			}

			for ( int i = 0 ; hand != null && i < hand.Fingers.Count ;  ++i ) {
				Finger finger = hand.Fingers[i];
				GameObject fingerObj = vFingerObjList[i];

				Vector3 tip = finger.TipPosition.ToUnityScaled();
				Vector3 dir = finger.Bone(Bone.BoneType.TYPE_DISTAL).Direction.ToUnity();

				fingerObj.transform.localPosition = tip;
				fingerObj.transform.localRotation = Quaternion.FromToRotation(Vector3.back, dir);
			}


		}

	}

}
