using Hover.Cast.Input;
using Hover.Common.Devices.Leap;
using Leap;
using UnityEngine;

namespace Hover.Cast.Devices.Leap.Touch {

	/*================================================================================================*/
	public class LeapInputCursor : IInputCursor {

		public bool IsLeft { get; private set; }
		public bool IsAvailable { get; private set; }

		public Vector3 Position { get; private set; }
		public Quaternion Rotation { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public LeapInputCursor(bool pIsLeft) {
			IsLeft = pIsLeft;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Rebuild(Finger pLeapFinger) {
			if ( pLeapFinger == null ) {
				IsAvailable = false;
				Position = Vector3.zero;
				Rotation = Quaternion.identity;
				return;
			}

			Bone bone = pLeapFinger.Bone(Bone.BoneType.TYPE_DISTAL);

			IsAvailable = true;
			Position = pLeapFinger.TipPosition.ToUnityScaled();
			Rotation = LeapInputMenu.CalcQuaternion(bone.Basis);
		}

	}

}
