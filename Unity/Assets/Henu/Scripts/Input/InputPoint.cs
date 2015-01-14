using Leap;
using UnityEngine;

namespace Henu.Input {

	/*================================================================================================*/
	public class InputPoint : IInputPoint {

		public Vector3 Position { get; private set; }
		public Quaternion Rotation { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public InputPoint(Finger pLeapFinger) {
			Bone bone = pLeapFinger.Bone(Bone.BoneType.TYPE_DISTAL);

			Position = pLeapFinger.TipPosition.ToUnityScaled();
			Rotation = InputHand.CalcQuaternion(bone.Basis);
		}

	}

}
