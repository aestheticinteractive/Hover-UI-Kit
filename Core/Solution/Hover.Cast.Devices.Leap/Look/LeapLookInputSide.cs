using Hover.Cast.Devices.Leap.Touch;
using Hover.Cast.Input;

namespace Hover.Cast.Devices.Leap.Look {

	/*================================================================================================*/
	public class LeapLookInputSide : LeapInputSide {

		private readonly LeapLookInputCursor vCursor;



		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public LeapLookInputSide(bool pIsLeft, LeapLookInputSettings pSettings) : 
																			base(pIsLeft, pSettings) {
			vCursor = new LeapLookInputCursor(pIsLeft, pSettings);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override IInputCursor Cursor {
			get {
				if ( vIsCursorStale ) {
					vCursor.Rebuild();
					vIsCursorStale = false;
				}

				return vCursor;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetOppositeHandMenu(IInputMenu pMenu) {
			vCursor.SetOppositeHandMenu(pMenu);
		}

	}

}
