using Hoverboard.Core.Custom;
using Hoverboard.Core.Input;
using UnityEngine;

namespace Hoverboard.Core.State {

	/*================================================================================================*/
	public class CursorState {

		public CursorType CursorType { get; private set; }
		public bool IsInputAvailable { get; private set; }
		public Vector3 Position { get; private set; }
		public float Size { get; private set; }

		private readonly InteractionSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public CursorState(InteractionSettings pSettings) {
			vSettings = pSettings;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void UpdateAfterInput(IInputCursor pInputCursor) {
			CursorType = pInputCursor.Type;
			IsInputAvailable = pInputCursor.IsAvailable;
			Size = pInputCursor.Size;

			Position = pInputCursor.Position+
				pInputCursor.Rotation*Vector3.back*vSettings.CursorForwardDistance;
		}

	}

}
