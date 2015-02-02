using Hovercast.Core.Input;
using Leap;
using UnityEngine;

namespace Hovercast.Devices.Leap {

	/*================================================================================================*/
	public class LeapInputPoint : IInputPoint {

		public Vector3 Position { get; private set; }
		public Quaternion Rotation { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public LeapInputPoint(Finger pLeapFinger) {
			Bone bone = pLeapFinger.Bone(Bone.BoneType.TYPE_DISTAL);

			Position = pLeapFinger.TipPosition.ToUnityScaled();
			Rotation = LeapInputCenter.CalcQuaternion(bone.Basis);
		}

	}

}
