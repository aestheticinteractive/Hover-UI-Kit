using Hovercast.Core.Input;
using Hovercast.Core.Settings;
using UnityEngine;

namespace Hovercast.Core.State {

	/*================================================================================================*/
	public class CursorState {

		public bool IsLeft { get; private set; }
		//public Vector3 PalmDirection { get; private set; }
		public Vector3? Position { get; private set; }

		private readonly IInputProvider vInputProv;
		private readonly InteractionSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public CursorState(IInputProvider pInputProv, InteractionSettings pSettings) {
			vInputProv = pInputProv;
			vSettings = pSettings;

			IsLeft = vSettings.IsMenuOnLeftSide;
			//PalmDirection = vInputProv.PalmDirection;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void UpdateAfterInput() {
			IsLeft = !vSettings.IsMenuOnLeftSide;

			IInputSide inputSide = vInputProv.GetSide(IsLeft);
			IInputCursor inputCursor = inputSide.Cursor;

			if ( inputCursor == null ) {
				Position = null;
				return;
			}

			Position = inputCursor.Position+
				inputCursor.Rotation*Vector3.back*vSettings.CursorForwardDistance;
		}

	}

}
