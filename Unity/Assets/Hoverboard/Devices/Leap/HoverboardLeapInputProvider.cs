using System;
using System.Collections.Generic;
using System.Linq;
using Hoverboard.Core.Input;
using Leap;

namespace Hoverboard.Devices.Leap {

	/*================================================================================================*/
	public class HoverboardLeapInputProvider : HoverboardInputProvider {

		public Finger.FingerType CursorPrimaryFinger = Finger.FingerType.TYPE_INDEX;
		public Finger.FingerType CursorSecondaryFinger = Finger.FingerType.TYPE_THUMB;
		public bool UseSecondary;

		private HandController vHandControl;
		protected LeapInputSettings vSettings;
		protected IDictionary<CursorType, LeapInputCursor> vCursorMap;
		private Frame vFrame;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Awake() {
			vHandControl = gameObject.GetComponent<HandController>();

			if ( vHandControl == null ) {
				throw new Exception("The HoverboardLeapInputProvider component must be added to the "+
					"same GameObject that contains the Leap Motion HandController component.");
			}

			vSettings = new LeapInputSettings();
			UpdateSettings();

			vCursorMap = new Dictionary<CursorType, LeapInputCursor>();
			vCursorMap[CursorType.PrimaryLeft] = 
				new LeapInputCursor(CursorType.PrimaryLeft, CursorPrimaryFinger);
			vCursorMap[CursorType.SecondaryLeft] = 
				new LeapInputCursor(CursorType.SecondaryLeft, CursorSecondaryFinger);
			vCursorMap[CursorType.PrimaryRight] = 
				new LeapInputCursor(CursorType.PrimaryRight, CursorPrimaryFinger);
			vCursorMap[CursorType.SecondaryRight] =
				new LeapInputCursor(CursorType.SecondaryRight, CursorSecondaryFinger);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void UpdateInput() {
			UpdateSettings();

			Frame frame = vHandControl.GetFrame();

			vFrame = (frame != null && frame.IsValid ? frame : null);

			Hand leapHandL = GetLeapHand(true);
			Hand leapHandR = GetLeapHand(false);

			if ( !IsEnabled ) {
				leapHandL = null;
				leapHandR = null;
			}

			vCursorMap[CursorType.PrimaryLeft].Rebuild(leapHandL);
			vCursorMap[CursorType.PrimaryRight].Rebuild(leapHandR);

			vCursorMap[CursorType.SecondaryLeft].Rebuild(UseSecondary ? leapHandL : null);
			vCursorMap[CursorType.SecondaryRight].Rebuild(UseSecondary ? leapHandR : null);
		}

		/*--------------------------------------------------------------------------------------------*/
		public override IInputCursor GetCursor(CursorType pType) {
			return vCursorMap[pType];
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateSettings() {
			vSettings.CursorPrimaryFinger = CursorPrimaryFinger;
			vSettings.CursorSecondaryFinger = CursorSecondaryFinger;
			vSettings.UseSecondary = UseSecondary;
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
