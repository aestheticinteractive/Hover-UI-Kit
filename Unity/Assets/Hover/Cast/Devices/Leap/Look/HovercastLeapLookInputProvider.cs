using System;
using Hover.Cast.Devices.Leap.Touch;
using UnityEngine;

namespace Hover.Cast.Devices.Leap.Look {

	/*================================================================================================*/
	public class HovercastLeapLookInputProvider : HovercastLeapInputProvider {

		public Transform HeadsetCameraTransform;
		public float CursorHorizontalOffset = 0.4f;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Awake() {
			base.Awake();

			if ( HeadsetCameraTransform == null ) {
				throw new Exception("The HovercastLeapLookInputProvider component requires the "+
					"'Headset Camera Transform' to be set.");
			}

			var sett = new LeapLookInputSettings();
			sett.LeapTransform = gameObject.transform;
			vSettings = sett;
			UpdateSettings();

			var sideL = new LeapLookInputSide(true, sett);
			var sideR = new LeapLookInputSide(false, sett);

			sideL.SetOppositeHandMenu(sideR.Menu);
			sideR.SetOppositeHandMenu(sideL.Menu);

			vSideL = sideL;
			vSideR = sideR;
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateSettings() {
			base.UpdateSettings();

			LeapLookInputSettings sett = (vSettings as LeapLookInputSettings);

			if ( sett == null ) { //will only happen upon first UpdateSettings() call
				return;
			}

			sett.CameraTransform = HeadsetCameraTransform;
			sett.CursorHorizontalOffset = CursorHorizontalOffset;
		}

	}

}
