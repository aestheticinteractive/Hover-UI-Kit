using Henu.Input;
using UnityEngine;

namespace Henu.State {

	/*================================================================================================*/
	public class CursorState {

		public Vector3? Position { get; private set; }

		private readonly IInputHandProvider vInputHandProv;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public CursorState(IInputHandProvider pInputHandProv) {
			vInputHandProv = pInputHandProv;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			IInputPoint inputPoint = vInputHandProv.IndexPoint;
			Position = (inputPoint == null ? (Vector3?)null : inputPoint.Position);
		}

	}

}
