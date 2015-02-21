using Hovercast.Core.Custom;
using Hovercast.Core.Input;
using UnityEngine;

namespace Hovercast.Core.State {

	/*================================================================================================*/
	public class CursorState {

		public bool IsInputAvailable { get; private set; }
		public bool IsLeft { get; private set; }
		public Vector3 Position { get; private set; }

		private readonly InteractionSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public CursorState(InteractionSettings pSettings) {
			vSettings = pSettings;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void UpdateAfterInput(IInputCursor pInputCursor) {
			IsLeft = pInputCursor.IsLeft;
			IsInputAvailable = pInputCursor.IsAvailable;

			Position = pInputCursor.Position+
				pInputCursor.Rotation*Vector3.back*vSettings.CursorForwardDistance;
		}

	}

}
