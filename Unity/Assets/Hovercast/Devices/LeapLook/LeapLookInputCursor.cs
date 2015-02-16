using Hovercast.Core.Input;
using UnityEngine;

namespace Hovercast.Devices.LeapLook {

	/*================================================================================================*/
	internal class LeapLookInputCursor : IInputCursor {

		public bool IsLeft { get; private set; }
		public bool IsActive { get; private set; }

		public Vector3 Position { get; private set; }
		public Quaternion Rotation { get; private set; }

		private readonly Transform vCameraTx;
		private readonly Transform vLeapTx;

		private IInputMenu vOppositeHandMenu;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public LeapLookInputCursor(bool pIsLeft, Transform pCameraTx, Transform pLeapTx) {
			IsLeft = pIsLeft;
			vCameraTx = pCameraTx;
			vLeapTx = pLeapTx;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetOppositeHandMenu(IInputMenu pMenu) {
			vOppositeHandMenu = pMenu;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Rebuild() {
			if ( !vOppositeHandMenu.IsActive ) {
				IsActive = false;
				Position = Vector3.zero;
				return;
			}

			//TODO: "planePos" is shifted forward slighty to negate the effect of UiLevel.PushFromHand.
			//This isn't exact. Could probably improve this by moving this "push" effect into the Leap
			//input module. That way, the menu would appear exactly where the input data specifies. It
			//becomes the input module's responsibility to offset the menu position to provide enough
			//space for things like 3D hand models.

			Vector3 camPos = vLeapTx.InverseTransformPoint(vCameraTx.position);
			Vector3 camDir = vLeapTx.InverseTransformDirection(vCameraTx.rotation*Vector3.forward);
			Vector3 planeNorm = vOppositeHandMenu.Rotation*Vector3.up;
			Vector3 planePos = vOppositeHandMenu.Position - planeNorm*0.025f;
			float numer = Vector3.Dot(planePos-camPos, planeNorm);
			float denom = Vector3.Dot(camDir, planeNorm);

			if ( denom == 0 ) { //exactly parallel (very unlikely scenario)
				IsActive = false;
				Position = Vector3.zero;
				return;
			}

			float t = numer/denom;

			IsActive = true;
			Position = camPos+camDir*t;
		}

	}

}
