using HandMenu.Input;
using UnityEngine;

namespace HandMenu.State {

	/*================================================================================================*/
	public class SelectHandState {

		public Vector3? CursorPosition { get; private set; }

		private readonly HandProvider vHandProv;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public SelectHandState(HandProvider pHandProv) {
			vHandProv = pHandProv;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterInput() {
			PointProvider pointProv = vHandProv.GetPointProvider(PointData.PointZone.Index);
			CursorPosition = (pointProv.Data == null ? (Vector3?)null : pointProv.Data.Position);
		}

	}

}
