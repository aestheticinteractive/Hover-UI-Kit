using Henu.Input;
using Henu.Settings;
using UnityEngine;

namespace Henu.State {

	/*================================================================================================*/
	public class CursorState {

		public Vector3? Position { get; private set; }

		private readonly IInputHandProvider vInputHandProv;
		private readonly InteractionSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public CursorState(IInputHandProvider pInputHandProv, InteractionSettings pSettings) {
			vInputHandProv = pInputHandProv;
			vSettings = pSettings;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void UpdateAfterInput() {
			IInputPoint inputPoint = vInputHandProv.IndexPoint;

			if ( inputPoint == null ) {
				Position = null;
				return;
			}

			Position = inputPoint.Position+
				inputPoint.Rotation*Vector3.back*vSettings.CursorForwardDistance;
		}

	}

}
