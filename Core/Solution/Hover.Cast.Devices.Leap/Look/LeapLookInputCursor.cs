using Hover.Cast.Input;
using UnityEngine;

namespace Hover.Cast.Devices.Leap.Look {

	/*================================================================================================*/
	public class LeapLookInputCursor {

		public bool IsLeft { get; private set; }
		public bool IsAvailable { get; private set; }

		public Vector3 Position { get; private set; }
		public Quaternion Rotation { get; private set; }

		private readonly LookInputSettings vSettings;
		private IInputMenu vOppositeHandMenu;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public LeapLookInputCursor(bool pIsLeft, LookInputSettings pSettings) {
			IsLeft = pIsLeft;
			vSettings = pSettings;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetOppositeHandMenu(IInputMenu pMenu) {
			vOppositeHandMenu = pMenu;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Rebuild() {
			if ( !vOppositeHandMenu.IsAvailable ) {
				IsAvailable = false;
				Position = Vector3.zero;
				return;
			}

			//TODO: "planePos" is shifted forward slighty to negate the effect of UiLevel.PushFromHand.
			//This isn't exact. Could probably improve this by moving this "push" effect into the Leap
			//input module. That way, the menu would appear exactly where the input data specifies. It
			//becomes the input module's responsibility to offset the menu position to provide enough
			//space for things like 3D hand models.

			Transform leapTx = vSettings.LeapTransform;
			Transform camTx = vSettings.CameraTransform;

			Vector3 camPos = leapTx.InverseTransformPoint(camTx.position);
			Vector3 camDir = leapTx.InverseTransformDirection(camTx.rotation*Vector3.forward);
			Vector3 camHoriz = leapTx.InverseTransformDirection(camTx.rotation*Vector3.left);

			camPos += camHoriz*vSettings.CursorHorizontalOffset*(IsLeft ? 1 : -1)*0.1f;
			
			Vector3 planeNorm = vOppositeHandMenu.Rotation*Vector3.up;
			Vector3 planePos = vOppositeHandMenu.Position - planeNorm*0.025f;
			
			float numer = Vector3.Dot(planePos-camPos, planeNorm);
			float denom = Vector3.Dot(camDir, planeNorm);

			if ( denom == 0 ) { //exactly parallel (very unlikely scenario)
				IsAvailable = false;
				Position = Vector3.zero;
				return;
			}

			float t = numer/denom;

			IsAvailable = true;
			Position = camPos+camDir*t;
		}

	}

}
