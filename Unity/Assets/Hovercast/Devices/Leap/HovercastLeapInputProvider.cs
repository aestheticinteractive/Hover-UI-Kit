using System;
using System.Linq;
using Hovercast.Core;
using Hovercast.Core.Input;
using Leap;
using UnityEngine;

namespace Hovercast.Devices.Leap {

	/*================================================================================================*/
	public class HovercastLeapInputProvider : HovercastInputProvider {

		public Vector3 ActivePalmDirection = Vector3.down;
		public Finger.FingerType CursorFinger = Finger.FingerType.TYPE_INDEX;
		public float NavigationBackGrabThreshold = 0.3f;
		public float NavigationBackUngrabThreshold = 0.15f;

		private HandController vHandControl;
		private LeapInputSettings vSettings;
		private LeapInputSide vSideL;
		private LeapInputSide vSideR;
		private Frame vFrame;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vHandControl = gameObject.GetComponent<HandController>();

			if ( vHandControl == null ) {
				throw new Exception("The HovercastLeapInputProvider component must be added to the "+
					"same GameObject that contains the Leap Motion HandController component.");
			}

			vSettings = new LeapInputSettings();
			UpdateSettings();

			vSideL = new LeapInputSide(true, vSettings);
			vSideR = new LeapInputSide(false, vSettings);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void UpdateInput() {
			UpdateSettings();

			Frame frame = vHandControl.GetFrame();

			vFrame = (frame != null && frame.IsValid ? frame : null);
			vSideL.UpdateWithLeapHand(GetLeapHand(true));
			vSideR.UpdateWithLeapHand(GetLeapHand(false));
		}

		/*--------------------------------------------------------------------------------------------*/
		public override IInputSide GetSide(bool pIsLeft) {
			return (pIsLeft ? vSideL : vSideR);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateSettings() {
			vSettings.PalmDirection = ActivePalmDirection;
			vSettings.CursorFinger = CursorFinger;
			vSettings.NavBackGrabThreshold = NavigationBackGrabThreshold;
			vSettings.NavBackUngrabThreshold = NavigationBackUngrabThreshold;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private Hand GetLeapHand(bool pIsLeft) {
			if ( vFrame == null ) {
				return null;
			}

			return vFrame.Hands.FirstOrDefault(h => h.IsValid && h.IsLeft == pIsLeft);
		}

	}

}
