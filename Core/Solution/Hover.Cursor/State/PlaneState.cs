using UnityEngine;

namespace Hover.Cursor.State {

	/*================================================================================================*/
	public class PlaneState : IPlaneState {

		public PlaneData Data { get; private set; }

		public bool IsHit { get; set; }
		public Vector3 HitPosition { get; set; }
		public float HitDist { get; set; }
		public bool IsNearest { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Init(PlaneData pData) {
			Data = pData;
			IsHit = false;
			HitPosition = Vector3.zero;
			HitDist = 0;
			IsNearest = false;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Vector3 PointWorld {
			get {
				return Data.LocalTransform.position;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public Vector3 NormalWorld {
			get {
				return Data.LocalTransform.rotation*Data.LocalNormal;
			}
		}

	}

}
