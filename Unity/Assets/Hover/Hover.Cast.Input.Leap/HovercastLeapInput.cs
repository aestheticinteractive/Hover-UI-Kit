using Hover.Common.Input.Leap;
using Leap;
using UnityEngine;

namespace Hover.Cast.Input.Leap {

	/*================================================================================================*/
	public class HovercastLeapInput : HovercastInput {

		public Vector3 ActivePalmDirection = Vector3.down;
		public float DistanceFromPalm = 0.2f;
		public float NavigationBackGrabThreshold = 0.3f;
		public float NavigationBackUngrabThreshold = 0.15f;

		private Controller vLeapControl;
		private InputSettings vSettings;
		private InputMenu vMenuL;
		private InputMenu vMenuR;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Awake() {
			vLeapControl = new Controller();

			vSettings = new InputSettings();
			vSettings.PalmDirection = ActivePalmDirection;
			vSettings.DistanceFromPalm = DistanceFromPalm;
			vSettings.NavBackGrabThreshold = NavigationBackGrabThreshold;
			vSettings.NavBackUngrabThreshold = NavigationBackUngrabThreshold;

			vMenuL = new InputMenu(true, vSettings);
			vMenuR = new InputMenu(false, vSettings);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void UpdateInput() {
			Frame leapFrame = LeapUtil.GetValidLeapFrame(vLeapControl);

			vMenuL.Rebuild(LeapUtil.GetValidLeapHand(leapFrame, true));
			vMenuR.Rebuild(LeapUtil.GetValidLeapHand(leapFrame, false));
		}

		/*--------------------------------------------------------------------------------------------*/
		public override IInputMenu GetMenu(bool pIsLeft) {
			return (pIsLeft ? vMenuL : vMenuR);
		}

	}

}
