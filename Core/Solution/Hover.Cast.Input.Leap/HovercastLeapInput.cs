using System.Linq;
using Leap;
using UnityEngine;

namespace Hover.Cast.Input.Leap {

	/*================================================================================================*/
	public class HovercastLeapInput : HovercastInputProvider {

		public Vector3 ActivePalmDirection = Vector3.down;
		public Finger.FingerType CursorFinger = Finger.FingerType.TYPE_INDEX;
		public float NavigationBackGrabThreshold = 0.3f;
		public float NavigationBackUngrabThreshold = 0.15f;

		private Controller vLeapControl;
		private InputSettings vSettings;
		private InputMenu vMenuL;
		private InputMenu vMenuR;
		private Frame vFrame;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Awake() {
			vLeapControl = new Controller();

			vSettings = new InputSettings();
			UpdateSettings();

			vMenuL = new InputMenu(true, vSettings);
			vMenuR = new InputMenu(false, vSettings);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void UpdateInput() {
			UpdateSettings();

			Frame leapFrame = vLeapControl.Frame(0);

			vFrame = (leapFrame != null && leapFrame.IsValid ? leapFrame : null);

			vMenuL.Rebuild(GetLeapHand(true));
			vMenuR.Rebuild(GetLeapHand(false));
		}

		/*--------------------------------------------------------------------------------------------*/
		public override IInputMenu GetMenu(bool pIsLeft) {
			return (pIsLeft ? vMenuL : vMenuR);
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
