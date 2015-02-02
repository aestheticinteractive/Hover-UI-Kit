using Hovercast.Core.Input;
using Hovercast.Core.Settings;
using UnityEngine;

namespace Hovercast.Core.State {

	/*================================================================================================*/
	public class CursorState {

		public bool IsActive { get; private set; }
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
			IsActive = pInputCursor.IsActive;

			Position = pInputCursor.Position+
				pInputCursor.Rotation*Vector3.back*vSettings.CursorForwardDistance;
		}

	}

}
