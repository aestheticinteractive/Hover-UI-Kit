using Hover.Cast.Input;
using Leap;

namespace Hover.Cast.Devices.Leap {

	/*================================================================================================*/
	public class InputSide : IInputSide {

		public bool IsLeft { get; private set; }

		private readonly InputSettings vSettings;
		private readonly InputMenu vMenu;

		private Hand vLeapHand;
		private bool vIsMenuStale;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public InputSide(bool pIsLeft, InputSettings pSettings) {
			IsLeft = pIsLeft;
			vSettings = pSettings;

			vMenu = new InputMenu(IsLeft);
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
