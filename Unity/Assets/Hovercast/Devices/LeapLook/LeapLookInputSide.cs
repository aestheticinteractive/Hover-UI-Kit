using Hovercast.Core.Input;
using Hovercast.Devices.Leap;
using UnityEngine;

namespace Hovercast.Devices.LeapLook {

	/*================================================================================================*/
	internal class LeapLookInputSide : LeapInputSide {

		private readonly LeapLookInputCursor vCursor;



		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public LeapLookInputSide(bool pIsLeft, Transform pCameraTx, Transform pLeapTx,
											LeapInputSettings pSettings) : base(pIsLeft, pSettings) {
			vCursor = new LeapLookInputCursor(pIsLeft, pCameraTx, pLeapTx);
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
