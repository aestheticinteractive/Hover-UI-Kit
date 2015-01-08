using HandMenu.Input;
using UnityEngine;

namespace HandMenu.State {

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
				vInputHandProv.GetPointProvider(InputPointData.PointZone.Index);

			CursorPosition = (pointProv.Data == null ? (Vector3?)null : pointProv.Data.Position);
		}

	}

}
