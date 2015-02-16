using Hovercast.Core.Input;
using Hovercast.Devices.Leap;

namespace Hovercast.Devices.LeapLook {

	/*================================================================================================*/
	internal class LeapLookInputSide : LeapInputSide {

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
