using Hover.Common.Input.Leap;
using Leap;

namespace Hover.Cast.Input.Leap {

	/*================================================================================================*/
	public class HovercastLeapInput : HovercastInput {

		public float DistanceFromPalm = 0.2f;
		public float NavigationBackGrabThreshold = 0.3f;
		public float NavigationBackUngrabThreshold = 0.15f;

		private LeapProvider vLeapProvider;
		private InputSettings vSettings;
		private InputMenu vMenuL;
		private InputMenu vMenuR;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Awake() {
			vLeapProvider = GetComponent<LeapProvider>();

			vSettings = new InputSettings();
			vSettings.DistanceFromPalm = DistanceFromPalm;
			vSettings.NavBackGrabThreshold = NavigationBackGrabThreshold;
			vSettings.NavBackUngrabThreshold = NavigationBackUngrabThreshold;

			vMenuL = new InputMenu(true, vSettings);
			vMenuR = new InputMenu(false, vSettings);
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void SetCameraTransform(UnityEngine.Transform pCameraTx) {
			vSettings.CameraTransform = pCameraTx;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void UpdateInput() {
			Frame leapFrame = vLeapProvider.CurrentFrame;

			vMenuL.Rebuild(LeapUtil.GetValidLeapHand(leapFrame, true));
			vMenuR.Rebuild(LeapUtil.GetValidLeapHand(leapFrame, false));
		}

		/*--------------------------------------------------------------------------------------------*/
		public override IInputMenu GetMenu(bool pIsLeft) {
			return (pIsLeft ? vMenuL : vMenuR);
		}

	}

}
