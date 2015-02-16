using System;
using Hovercast.Devices.Leap;
using UnityEngine;

namespace Hovercast.Devices.LeapLook {

	/*================================================================================================*/
	public class HovercastLeapLookInputProvider : HovercastLeapInputProvider {

		public Transform HeadsetCameraTransform;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Awake() {
			base.Awake();

			if ( HeadsetCameraTransform == null ) {
				throw new Exception("The HovercastLeapLookInputProvider component requires the "+
					"'Headset Camera Transform' to be set.");
			}

			Transform leapTx = gameObject.transform;

			var sideL = new LeapLookInputSide(true, HeadsetCameraTransform, leapTx, vSettings);
			var sideR = new LeapLookInputSide(false, HeadsetCameraTransform, leapTx, vSettings);

			sideL.SetOppositeHandMenu(sideR.Menu);
			sideR.SetOppositeHandMenu(sideL.Menu);

			vSideL = sideL;
			vSideR = sideR;
		}

	}

}
