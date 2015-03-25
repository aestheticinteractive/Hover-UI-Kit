using System;
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
				throw new Exception("The "+typeof(HovercastLeapLookInputProvider)+" component "+
					"requires the 'Headset Camera Transform' to be set.");
			}

			/*var sett = new LookInputSettings();
			sett.LeapTransform = gameObject.transform;
			vSettings = sett;
			UpdateSettings();

			var sideL = new LookInputSide(true, sett);
			var sideR = new LookInputSide(false, sett);

			sideL.SetOppositeHandMenu(sideR.Menu);
			sideR.SetOppositeHandMenu(sideL.Menu);

			vSideL = sideL;
			vSideR = sideR;*/
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateSettings() {
			base.UpdateSettings();

			/*LookInputSettings sett = (vSettings as LookInputSettings);

			if ( sett == null ) { //will only happen upon first UpdateSettings() call
				return;
			}

			sett.CameraTransform = HeadsetCameraTransform;
			sett.CursorHorizontalOffset = CursorHorizontalOffset;*/
		}

	}

}
