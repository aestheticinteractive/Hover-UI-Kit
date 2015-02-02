using System.Linq;
using Hovercast.Core.Input;
using Leap;

namespace Hovercast.Devices.Leap {

	/*================================================================================================*/
	internal class LeapInputSide : IInputSide {

		public bool IsLeft { get; private set; }

		private readonly LeapInputSettings vSettings;
		private readonly LeapInputMenu vMenu;
		private readonly LeapInputCursor vCursor;

		private Hand vLeapHand;
		private bool vIsMenuStale;
		private bool vIsCursorStale;


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
				if ( vIsMenuStale ) {
					vMenu.Rebuild(vLeapHand, vSettings);
					vIsMenuStale = false;
				}

				return vMenu;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public IInputCursor Cursor {
			get {
				if ( vIsCursorStale ) {
					vCursor.Rebuild(GetCursorLeapFinger());
					vIsCursorStale = false;
				}

				return vCursor;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void UpdateWithLeapHand(Hand pLeapHand) {
			vLeapHand = (pLeapHand != null && pLeapHand.IsValid ? pLeapHand : null);
			vIsMenuStale = true;
			vIsCursorStale = true;
		}

		/*--------------------------------------------------------------------------------------------*/
		private Finger GetCursorLeapFinger() {
			if ( vLeapHand == null ) {
				return null;
			}

			return vLeapHand.Fingers
				.FingerType(vSettings.CursorFinger)
				.FirstOrDefault(f => f.IsValid);
		}

	}

}
