using Leap;
using UnityEngine;

namespace HandMenu {

	/*================================================================================================*/
	public class FingerData {

		public Vector3 Position;
		public Vector3 Direction;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public FingerData() {
		}

		/*--------------------------------------------------------------------------------------------*/
		public FingerData(Finger pFinger) {
			Position = pFinger.TipPosition.ToUnityScaled();
			Direction = pFinger.Bone(Bone.BoneType.TYPE_DISTAL).Direction.ToUnity();
		}

	}

}
