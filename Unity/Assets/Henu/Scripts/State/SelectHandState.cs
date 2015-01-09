using Henu.Input;
using UnityEngine;

namespace Henu.State {

	/*================================================================================================*/
	public class SelectHandState {

		public Vector3? CursorPosition { get; private set; }

		private readonly InputHandProvider vInputHandProv;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public SelectHandState(InputHandProvider pInputHandProv) {
			vInputHandProv = pInputHandProv;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			InputPointProvider pointProv = 
				vInputHandProv.GetPointProvider(InputPointZone.Index);

			CursorPosition = (pointProv.Point == null ? (Vector3?)null : pointProv.Point.Position);
		}

	}

}
