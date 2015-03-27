using UnityEngine;

namespace Hover.Cursor.State {

	/*================================================================================================*/
	public struct PlaneData {

		//TODO: FEATURE: allow boundaries on the plane. Possibly do this via a custom "hit test" ...
		// ... function within InteractionPlaneState.

		public readonly string Id;
		public readonly Transform LocalTransform;
		public readonly Vector3 LocalNormal;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public PlaneData(string pId, Transform pLocalTransform, Vector3 pLocalNormal) {
			Id = pId;
			LocalTransform = pLocalTransform;
			LocalNormal = pLocalNormal;
		}

	}

}
