using System.Linq;
using Hovercast.Core.Input;
using Leap;

namespace Hovercast.Devices.Leap {

	/*================================================================================================*/
	internal class LeapInputSide : IInputSide {

		public bool IsLeft { get; private set; }
		public bool IsActive { get; private set; }

		private readonly LeapInputSettings vSettings;
		private readonly LeapInputMenu vMenu;
		private readonly LeapInputCursor vCursor;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public LeapInputSide(bool pIsLeft, LeapInputSettings pSettings) {
			IsLeft = pIsLeft;
			vSettings = pSettings;

			vMenu = new LeapInputMenu(IsLeft);
			vCursor = new LeapInputCursor(IsLeft);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IInputMenu Menu {
			get {
				return (IsActive ? vMenu : null);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public IInputCursor Cursor {
			get {
				return (IsActive ? vCursor : null);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void UpdateWithLeapHand(Hand pLeapHand) {
			bool isHandValid = (pLeapHand != null && pLeapHand.IsValid);
			IsActive = isHandValid;

			if ( !isHandValid ) {
				return;
			}

			Finger leapFinger = pLeapHand.Fingers
				.FingerType(vSettings.CursorFinger)
				.FirstOrDefault(f => f.IsValid);

			vMenu.Rebuild(pLeapHand, vSettings);
			vCursor.Rebuild(leapFinger);
		}

	}

}
