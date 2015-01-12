using Henu.Input;
using UnityEngine;

namespace Henu.State {

	/*================================================================================================*/
	public class CursorState {

		public Vector3? Position { get; private set; }

		private readonly InputHandProvider vInputHandProv;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public CursorState(InputHandProvider pInputHandProv) {
			vInputHandProv = pInputHandProv;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			InputPoint inputPoint = vInputHandProv.IndexPoint;
			Position = (inputPoint == null ? (Vector3?)null : inputPoint.Position);
		}

	}

}
