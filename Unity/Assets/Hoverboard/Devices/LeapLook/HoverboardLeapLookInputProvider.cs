using System;
using Hoverboard.Core.Input;
using UnityEngine;

namespace Hoverboard.Devices.LeapLook {

	/*================================================================================================*/
	public class HoverboardLeapLookInputProvider : HoverboardInputProvider {

		public Transform HeadsetCameraTransform;
		public float CursorHorizontalOffset = 0.4f;

		protected LeapLookInputSettings vSettings;
		protected LeapLookInputCursor vCursor;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( HeadsetCameraTransform == null ) {
				throw new Exception("The HovercastLeapLookInputProvider component requires the "+
					"'Headset Camera Transform' to be set.");
			}

			vSettings = new LeapLookInputSettings();
			vSettings.LeapTransform = gameObject.transform;
			UpdateSettings();

			vCursor = new LeapLookInputCursor(vSettings);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void UpdateInput() {
			UpdateSettings();
		}

		/*--------------------------------------------------------------------------------------------*/
		public override IInputCursor GetCursor(CursorType pType) {
			if ( pType == CursorType.PrimaryLeft ) {
				return vCursor;
			}

			return null;
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateSettings() {
			vSettings.CameraTransform = HeadsetCameraTransform;
			vSettings.CursorHorizontalOffset = CursorHorizontalOffset;
		}

	}

}
