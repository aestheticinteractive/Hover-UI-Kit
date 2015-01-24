using Hovercast.Input;
using Hovercast.Settings;
using UnityEngine;

namespace Hovercast.State {

	/*================================================================================================*/
	public class CursorState {

		public bool IsLeft { get; private set; }
		public Vector3 PalmDirection { get; private set; }
		public Vector3? Position { get; private set; }

		private readonly IInputProvider vInputProv;
		private readonly InteractionSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public CursorState(IInputProvider pInputProv, InteractionSettings pSettings) {
			vInputProv = pInputProv;
			vSettings = pSettings;

			IsLeft = vSettings.IsMenuOnLeftSide;
			PalmDirection = vInputProv.PalmDirection;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void UpdateAfterInput() {
			IsLeft = !vSettings.IsMenuOnLeftSide;

			IInputSide inputSide = vInputProv.GetSide(IsLeft);
			IInputPoint inputPoint = inputSide.Points[0];

			if ( inputPoint == null ) {
				Position = null;
				return;
			}

			Position = inputPoint.Position+
				inputPoint.Rotation*Vector3.back*vSettings.CursorForwardDistance;
		}

	}

}
