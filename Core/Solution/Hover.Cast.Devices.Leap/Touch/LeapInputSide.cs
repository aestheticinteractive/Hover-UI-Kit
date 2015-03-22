using System.Linq;
using Hover.Cast.Input;
using Leap;

namespace Hover.Cast.Devices.Leap.Touch {

	/*================================================================================================*/
	public class LeapInputSide : IInputSide {

		public bool IsLeft { get; private set; }

		private readonly LeapInputSettings vSettings;
		private readonly LeapInputMenu vMenu;

		private Hand vLeapHand;
		private bool vIsMenuStale;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public LeapInputSide(bool pIsLeft, LeapInputSettings pSettings) {
			IsLeft = pIsLeft;
			vSettings = pSettings;

			vMenu = new LeapInputMenu(IsLeft);
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


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateWithLeapHand(Hand pLeapHand) {
			vLeapHand = (pLeapHand != null && pLeapHand.IsValid ? pLeapHand : null);
			vIsMenuStale = true;
		}

	}

}
