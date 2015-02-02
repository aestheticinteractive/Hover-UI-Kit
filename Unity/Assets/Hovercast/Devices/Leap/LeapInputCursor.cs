using Hovercast.Core.Input;
using Leap;
using UnityEngine;

namespace Hovercast.Devices.Leap {

	/*================================================================================================*/
	internal class LeapInputCursor : IInputCursor {

		public bool IsLeft { get; private set; }
		public bool IsActive { get; private set; }

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
				IsActive = false;
				Position = Vector3.zero;
				Rotation = Quaternion.identity;
				return;
			}

			Bone bone = pLeapFinger.Bone(Bone.BoneType.TYPE_DISTAL);

			IsActive = true;
			Position = pLeapFinger.TipPosition.ToUnityScaled();
			Rotation = LeapInputMenu.CalcQuaternion(bone.Basis);
		}

	}

}
