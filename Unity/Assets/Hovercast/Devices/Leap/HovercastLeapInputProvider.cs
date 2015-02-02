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

		private HandController vHandControl;
		private Frame vFrame;
		private LeapInputSide vSideL;
		private LeapInputSide vSideR;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vHandControl = gameObject.GetComponent<HandController>();

			if ( vHandControl == null ) {
				throw new Exception("The HovercastLeapInputProvider component must be added to the "+
					"same GameObject that contains the Leap Motion HandController component.");
			}

			vSideL = new LeapInputSide(true, PalmDirection);
			vSideR = new LeapInputSide(false, PalmDirection);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override Vector3 PalmDirection {
			get {
				return ActivePalmDirection.normalized;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override void UpdateInput() {
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
		private Hand GetLeapHand(bool pIsLeft) {
			if ( vFrame == null ) {
				return null;
			}

			return vFrame.Hands.FirstOrDefault(h => h.IsValid && h.IsLeft == pIsLeft);
		}

	}

}
