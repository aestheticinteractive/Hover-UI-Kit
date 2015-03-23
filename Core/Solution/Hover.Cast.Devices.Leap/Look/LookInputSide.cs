using Hover.Cast.Input;

namespace Hover.Cast.Devices.Leap.Look {

	/*================================================================================================*/
	public class LookInputSide : InputSide {

		private readonly LeapLookInputCursor vCursor;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public LookInputSide(bool pIsLeft, LookInputSettings pSettings) : 
																			base(pIsLeft, pSettings) {
			vCursor = new LeapLookInputCursor(pIsLeft, pSettings);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public object Cursor {
			get {
				//if ( vIsCursorStale ) {
					vCursor.Rebuild();
				//	vIsCursorStale = false;
				//}

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
