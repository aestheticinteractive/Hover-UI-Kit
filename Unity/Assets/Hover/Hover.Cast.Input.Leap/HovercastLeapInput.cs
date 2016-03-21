using Hover.Common.Input.Leap;
using Leap;

namespace Hover.Cast.Input.Leap {

	/*================================================================================================*/
	public class HovercastLeapInput : HovercastInput {

		public float DistanceFromPalm = 0.2f;
		public float NavigationBackGrabThreshold = 0.9f;
		public float NavigationBackUngrabThreshold = 0.5f;

		private readonly InputSettings vSettings;
		private LeapProvider vLeapProvider;
		private InputMenu vMenuL;
		private InputMenu vMenuR;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HovercastLeapInput() {
			vSettings = new InputSettings();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Awake() {
			vLeapProvider = GetComponent<LeapProvider>();

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

			vSettings.DistanceFromPalm = DistanceFromPalm;
			vSettings.NavBackGrabThreshold = NavigationBackGrabThreshold;
			vSettings.NavBackUngrabThreshold = NavigationBackUngrabThreshold;

			vMenuL.Rebuild(LeapUtil.GetValidLeapHand(leapFrame, true));
			vMenuR.Rebuild(LeapUtil.GetValidLeapHand(leapFrame, false));
		}

		/*--------------------------------------------------------------------------------------------*/
		public override IInputMenu GetMenu(bool pIsLeft) {
			return (pIsLeft ? vMenuL : vMenuR);
		}

	}

}
