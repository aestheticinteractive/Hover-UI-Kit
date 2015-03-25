using System.Collections.Generic;
using System.Linq;
using Hover.Common.Input;
using Leap;

namespace Hover.Cursor.Input.Leap.Hands {

	/*================================================================================================*/
	public class HovercursorLeapHandsInput : HovercursorInputProvider {

		private Controller vLeapControl;
		private InputSettings vSettings;
		private IDictionary<CursorType, InputCursor> vCursorMap;
		private IDictionary<CursorType, bool> vSideMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Awake() {
			vLeapControl = new Controller();
			vSettings = new InputSettings();
			vCursorMap = new Dictionary<CursorType, InputCursor>();
			vSideMap = new Dictionary<CursorType, bool>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void UpdateInput() {
			UpdateSettings();

			Frame frame = GetValidLeapFrame(vLeapControl);
			Hand leapHandL = GetValidLeapHand(frame, true);
			Hand leapHandR = GetValidLeapHand(frame, false);

			if ( !IsEnabled ) {
				leapHandL = null;
				leapHandR = null;
			}

			foreach ( InputCursor cursor in vCursorMap.Values ) {
				cursor.Rebuild(vSideMap[cursor.Type] ? leapHandL : leapHandR);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public override IInputCursor GetCursor(CursorType pType) {
			if ( !vCursorMap.ContainsKey(pType) ) {
				vCursorMap.Add(pType, new InputCursor(pType));
				vSideMap.Add(pType, CursorTypeUtil.IsLeft(pType));
			}

			return vCursorMap[pType];
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateSettings() {
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static Frame GetValidLeapFrame(Controller pLeapControl) {
			Frame frame = pLeapControl.Frame(0);
			return (frame != null && frame.IsValid ? frame : null);
		}

		/*--------------------------------------------------------------------------------------------*/
		private static Hand GetValidLeapHand(Frame pLeapFrame, bool pIsLeft) {
			if ( pLeapFrame == null ) {
				return null;
			}

			return pLeapFrame.Hands.FirstOrDefault(h => h.IsValid && h.IsLeft == pIsLeft);
		}

	}

}
