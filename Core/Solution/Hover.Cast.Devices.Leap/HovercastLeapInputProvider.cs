using System.Linq;
using Hover.Cast.Input;
using Leap;
using UnityEngine;

namespace Hover.Cast.Devices.Leap {

	/*================================================================================================*/
	public class HovercastLeapInputProvider : HovercastInputProvider {

		public Vector3 ActivePalmDirection = Vector3.down;
		public Finger.FingerType CursorFinger = Finger.FingerType.TYPE_INDEX;
		public float NavigationBackGrabThreshold = 0.3f;
		public float NavigationBackUngrabThreshold = 0.15f;

		private Controller vLeapControl;
		protected InputSettings vSettings;
		protected InputSide vSideL;
		protected InputSide vSideR;
		private Frame vFrame;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Awake() {
			vLeapControl = new Controller();

			vSettings = new InputSettings();
			UpdateSettings();

			vSideL = new InputSide(true, vSettings);
			vSideR = new InputSide(false, vSettings);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void UpdateInput() {
			UpdateSettings();

			Frame leapFrame = vLeapControl.Frame(0);

			vFrame = (leapFrame != null && leapFrame.IsValid ? leapFrame : null);
			vSideL.UpdateWithLeapHand(GetLeapHand(true));
			vSideR.UpdateWithLeapHand(GetLeapHand(false));
		}

		/*--------------------------------------------------------------------------------------------*/
		public override IInputSide GetSide(bool pIsLeft) {
			return (pIsLeft ? vSideL : vSideR);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateSettings() {
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
