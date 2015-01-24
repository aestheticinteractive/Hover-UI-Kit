using Leap;
using UnityEngine;

namespace Hovercast.Input {

	/*================================================================================================*/
	public class InputPoint : IInputPoint {

		public Vector3 Position { get; private set; }
		public Quaternion Rotation { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public InputPoint(Finger pLeapFinger) {
			Bone bone = pLeapFinger.Bone(Bone.BoneType.TYPE_DISTAL);

			Position = pLeapFinger.TipPosition.ToUnityScaled();
			Rotation = InputCenter.CalcQuaternion(bone.Basis);
		}

	}

}
